using AGS.Core.Classes.ViewScripts;
using AGS.Core.Examples.SystemExample;
using UnityEngine;

namespace AGS.Core.Examples.ExampleViewScripts
{
    public class MovingDiscStopper : ViewScriptBase
    {
        private PressurePlateView _pressurePlateView;
        private PressurePlate _pressurePlate;

        public MovingDiscView MovingDiscView;

        protected override void SetupModelBindings()
        {
            base.SetupModelBindings();
            if (ViewReference != null)
            {
                _pressurePlateView = ViewReference as PressurePlateView;
                if (_pressurePlateView != null)
                {
                    _pressurePlate = _pressurePlateView.PressurePlate;
                }
            }
            if (_pressurePlate == null) return;
            
            _pressurePlate.IsPressured.OnValueChanged += (sender, isPressured) =>
            {
                    if (isPressured.Value)
                    {
                        MovingDiscView.Stop();
                    }
                    else
                    {
                        MovingDiscView.Continue();
                    }
                
            };
        }
    }
}
