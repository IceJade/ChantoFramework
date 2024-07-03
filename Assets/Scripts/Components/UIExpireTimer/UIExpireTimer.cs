//----------------------------------------------------------------------------
// @说    明: UI倒计时组件
// @版 本 号: 1.0
// @创建时间: 2021.12.13
// @作    者: zhoumingfeng
// @使用说明: 
//----------------------------------------------------------------------------
using Framework;

namespace Chanto
{
    // 到期时间计时器
    public class UIExpireTimer : UITimerBase
    {
        //----------------------------------------------------------------
        // 编辑器参数

        // 文本框;
        public IMTextMeshProUGUI text;

        // 显示时是否进一位，比如：当前时间为10:10:10，如果只显示小时和分钟的话，勾选carryTime显示为10:11，不勾显示10:10
        public bool carryTime = true;

        // 最低显示位进位，即不显示0, 以1代替;
        public bool carryLastBit = false;

        // 显示的文本
        public int languageId = 0;
        public string textExpired = "";
        public string textDay = "{0}d {1:D2}:{2:D2}:{3:D2}";
        public string textHour = "{0:D2}:{1:D2}:{2:D2}";
        public string textMinute = "{0:D2}:{1:D2}";
        public string textSeconds = "{0}";

        // 时间单位标识
        public int unitDay = UNIT_DAY | UNIT_HOUR | UNIT_MINUTE | UNIT_SECOND;
        public int unitHour = UNIT_HOUR | UNIT_MINUTE | UNIT_SECOND;
        public int unitMinute = UNIT_MINUTE | UNIT_SECOND;
        public int unitSeconds = UNIT_SECOND;

        //----------------------------------------------------------------
        // 成员变量

        protected int m_textType = 0;
        protected long[] m_timeValues = null;
        protected string m_lastFormat = null;
        protected long[] m_lastTimeValues = null;
        protected string m_realDayFormat = null;
        protected string m_languageText = null;

        private string _flag_day = "d";

        //----------------------------------------------------------------
        // 公用函数

        public virtual void SetText(string strText)
        {
            string tmpTime = this.GetRealText(strText);

            if (null != text)
            {
                text.SetTextEx(tmpTime);
            }
            else
            {
                text = this.GetComponent<IMTextMeshProUGUI>();
                if (null != text)
                    text.SetTextEx(tmpTime);
                else
                    Log.Error("请挂载IMTextMeshProUGUI脚本再使用!");
            }
        }

        /// <summary>
        /// 定时器是否已经开始;
        /// </summary>
        public virtual bool IsTimerStarted()
        {
            return this.m_timerStarted;
        }

        public override void StopTimer(bool notify = true, bool resetTime = false, bool updateDisplay = true)
        {
            m_lastFormat = null;
            if (m_lastTimeValues != null)
            {
                for (int i = 0, imax = m_lastTimeValues.Length; i < imax; ++i)
                {
                    m_lastTimeValues[i] = 0;
                }
            }
            base.StopTimer(notify, resetTime, updateDisplay);
        }

        //----------------------------------------------------------------
        // 内部函数

        protected override void UpdateDisplay()
        {
            if (!this.IsTimerStarted())
                return;

            long ticks = m_leftTime.Ticks;
            if (ticks < 0) ticks = 0;
            if (ticks == 0 && !string.IsNullOrEmpty(textExpired))
            {
                SetText(textExpired);

                EndTimeCallBack?.Invoke();
            }
            else
            {
                bool result = false;
                string text = "";
                switch (m_textType)
                {
                    case UNIT_DAY:
                        result = FormatText(out text, ticks, this.GetTextDayFormat(), m_displayUnit);
                        break;
                    case UNIT_HOUR:
                        result = FormatText(out text, ticks, textHour, m_displayUnit);
                        break;
                    case UNIT_MINUTE:
                        result = FormatText(out text, ticks, textMinute, m_displayUnit);
                        break;
                    case UNIT_SECOND:
                        result = FormatText(out text, ticks, textSeconds, m_displayUnit);
                        break;
                    default:
                        text = textExpired;
                        break;
                }
                if (result)
                {
                    SetText(text);
                }
            }
        }

        protected override void UpdateDisplayUnit()
        {
            double leftSeconds = m_leftTime.TotalSeconds;
            if (leftSeconds >= 86400.0d)
            {
                m_textType = UNIT_DAY;
                SetDisplayUnit(unitDay);
            }
            else if (leftSeconds >= 3600.0d)
            {
                m_textType = UNIT_HOUR;
                SetDisplayUnit(unitHour);
            }
            else if (leftSeconds >= 60.0d)
            {
                m_textType = UNIT_MINUTE;
                SetDisplayUnit(unitMinute);
            }
            else
            {
                m_textType = UNIT_SECOND;
                SetDisplayUnit(unitSeconds);
            }
        }

        protected virtual bool FormatText(out string text, long ticks, string format, int displayUnit)
        {
            if (m_timeValues == null)
            {
                m_timeValues = new long[UNIT_COUNT];
            }

            long mod = 0;
            long unitTotalTicks = 0;
            if (carryTime)
            {
                for (int i = 0; i < UNIT_COUNT; ++i)
                {
                    if ((displayUnit & UNIT_ARRAY[i]) != 0)
                    {
                        unitTotalTicks = UNIT_TOTAL_TICKS[i];
                        mod = ticks % unitTotalTicks;
                        if (mod > 0)
                        {
                            ticks += unitTotalTicks - mod;
                        }
                        break;
                    }
                }
            }

            int valueCount = 0;
            long timeValue = 0;
            long sum = 0;
            for (int i = UNIT_COUNT - 1; i >= 0; --i)
            {
                if ((displayUnit & UNIT_ARRAY[i]) != 0)
                {
                    unitTotalTicks = UNIT_TOTAL_TICKS[i];
                    timeValue = (ticks - sum) / unitTotalTicks;
                    sum += timeValue * unitTotalTicks;
                    m_timeValues[valueCount++] = timeValue;
                }
                else
                {
                    if (carryLastBit && valueCount > 0 && m_timeValues[valueCount - 1] == 0)
                        m_timeValues[valueCount - 1] = 1;
                }
            }

            bool sameTimeValue = false;
            if (m_lastTimeValues == null)
            {
                m_lastTimeValues = new long[UNIT_COUNT];
            }
            else
            {
                sameTimeValue = true;
                for (int i = 0; i < UNIT_COUNT; ++i)
                {
                    if (m_lastTimeValues[i] != m_timeValues[i])
                    {
                        sameTimeValue = false;
                        break;
                    }
                }
            }

            if (sameTimeValue && m_lastFormat == format)
            {
                text = "";
                return false;
            }
            else
            {
                m_lastFormat = format;
                for (int i = 0; i < UNIT_COUNT; ++i)
                {
                    m_lastTimeValues[i] = m_timeValues[i];
                }
            }

            switch (valueCount)
            {
                case 4:
                    text = string.Format(format, m_timeValues[0], m_timeValues[1], m_timeValues[2], m_timeValues[3]);
                    break;
                case 3:
                    text = string.Format(format, m_timeValues[0], m_timeValues[1], m_timeValues[2]);
                    break;
                case 2:
                    text = string.Format(format, m_timeValues[0], m_timeValues[1]);
                    break;
                case 1:
                    text = string.Format(format, m_timeValues[0]);
                    break;
                default:
                    text = format;
                    break;
            }

            return true;
        }

        /// <summary>
        /// 获取包含天的格式
        /// </summary>
        /// <returns></returns>
        private string GetTextDayFormat()
        {
            if (!this.useLocalDay)
            {
                return this.textDay;
            }
            else
            {
                if (this.m_realDayFormat.IsNullOrEmpty())
                {
                    this.m_realDayFormat = this.textDay;

                    if (this.textDay.Contains(this._flag_day))
                    {
                        var day = GameEntry.Localization.GetString(370005);
                        if (!day.IsNullOrEmpty())
                            this.m_realDayFormat = this.m_realDayFormat.Replace(this._flag_day, day);
                    }
                }

                return this.m_realDayFormat;
            }
        }

        private string GetRealText(string time)
        {
            string realText = time;

            if (this.languageId > 0)
            {
                if (m_languageText.IsNullOrEmpty())
                {
                    m_languageText = GameEntry.Localization.GetString(this.languageId);

                    if (!m_languageText.Contains("{0}"))
                        m_languageText += "{0}";
                }
                else
                {
                    realText = string.Format(m_languageText, time);
                }
            }

            return realText;
        }
    }
}