using System.Collections.Generic;

namespace Framework
{
   
    public class ControllerContainer : Singleton<ControllerContainer>
    {
        
        private List<ControllerBase> _controllerBases = new List<ControllerBase>();
        private List<ControllerBase> _updateSecond = new List<ControllerBase>();

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

        public void AddControllerBase(ControllerBase controller)
        {
            _controllerBases.Add(controller);
        }
        
        public void RemoveControllerBase(ControllerBase controller)
        {
            if (_updateSecond.Contains(controller))
            {
                _updateSecond.Remove(controller);
            }
            if (_controllerBases.Contains(controller))
            {
                controller.DestroyInstance();
                _controllerBases.Remove(controller);
            }
           
        }
        
        public void DestroyAllControllerBase()
        {
            _updateSecond.Clear();
            foreach (var controller in _controllerBases)
            {
                controller.DestroyInstance();
            }
            _controllerBases.Clear();
        }
        
        public void ResetAllControllerBase()
        {
            foreach (var controller in _controllerBases)
            {
                controller.Reset();
            }
        }
        
        public void OnApplicationFocus(bool force)
        {
            foreach (var controller in _controllerBases)
            {
                controller.OnApplicationFocus(force);
            }
        }

        public void OnApplicationPause(bool pause)
        {
            foreach (var controller in _controllerBases)
            {
                controller.OnApplicationPause(pause);
            }
        }
    }
}