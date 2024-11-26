using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Framework
{
   
    public class ControllerManager : Singleton<ControllerManager>
    {
        private Dictionary<Type, ControllerBase> _controllerDic = new();
        private List<ControllerBase> _updateSecond = new();

        public void AddUpdateSecond(ControllerBase controller)
        {
            if (_updateSecond.Contains(controller))
            {
                Log.Error("AddUpdateSecond: Already in update second {controller.GetType().ToString()}");
                return; 
            }
            _updateSecond.Add(controller);
        }
        
        public void RemoveUpdateSecond(ControllerBase controller)
        {
            if (!_updateSecond.Contains(controller))
            {
                Log.Error("RemoveUpdateSecond: Unexist controller in update second {controller.GetType().ToString()}");
                return; 
            }
            _updateSecond.Remove(controller);
        }

        public void Instantiate()
        {
            // ��ȡ��ǰ����
            var assembly = Assembly.GetExecutingAssembly();

            // �������м̳��� ControllerBase ������
            var controllerTypes = assembly.GetTypes()
                                           .Where(t => t.IsSubclassOf(typeof(ControllerBase)) && !t.IsAbstract)
                                           .ToList();

            // ʵ������Щ����
            foreach (var controllerType in controllerTypes)
            {
                try
                {
                    var controllerInstance = Activator.CreateInstance(controllerType);
                    Log.Info($"ʵ������ {controllerType.Name} ��");
                }
                catch (Exception ex)
                {
                    Log.Info($"ʵ���� {controllerType.Name} ʱ����: {ex.Message}");
                }
            }
        }

        public void Init()
        {
            foreach (var controller in _controllerDic)
            {
                controller.Value.InitInstance();
            }
        }

        public bool IsUpdateSecond(ControllerBase controller)
        {
            return _updateSecond.Contains(controller);
        }
        
        public void UpdateSecond(float elapsedTime)
        {
            foreach (var controller in _updateSecond)
            {
                controller.UpdateSecond(elapsedTime);
            }
        }

        public void Add(ControllerBase controller)
        {
            _controllerDic.Add(controller.GetType(), controller);
        }
        
        public void Remove(ControllerBase controller)
        {
            if (_updateSecond.Contains(controller))
            {
                _updateSecond.Remove(controller);
            }
            if (_controllerDic.ContainsKey(controller.GetType()))
            {
                controller.DestroyInstance();
                _controllerDic.Remove(controller.GetType());
            }
           
        }
        
        public void Destroy()
        {
            _updateSecond.Clear();
            foreach (var controller in _controllerDic)
            {
                controller.Value.DestroyInstance();
            }
            _controllerDic.Clear();
        }
        
        public void ResetAllControllerBase()
        {
            foreach (var controller in _controllerDic)
            {
                controller.Value.Reset();
            }
        }
        
        public void OnApplicationFocus(bool force)
        {
            foreach (var controller in _controllerDic)
            {
                controller.Value.OnApplicationFocus(force);
            }
        }

        public void OnApplicationPause(bool pause)
        {
            foreach (var controller in _controllerDic)
            {
                controller.Value.OnApplicationPause(pause);
            }
        }
    }
}