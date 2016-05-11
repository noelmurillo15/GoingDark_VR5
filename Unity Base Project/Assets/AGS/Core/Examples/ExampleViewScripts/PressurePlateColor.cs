using AGS.Core.Classes.ViewScripts;
using AGS.Core.Examples.SystemExample;
using UnityEngine;

namespace AGS.Core.Examples.ExampleViewScripts
{
    public class PressurePlateColor : ViewScriptBase
    {
        private PressurePlateView _pressurePlateView;
        private PressurePlate _pressurePlate;
        private Light _light;
        private Renderer _renderer;
        public override void Awake()
        {
            base.Awake();
            _renderer = GetComponent<Renderer>();
            _light = GetComponentInChildren<Light>();
        }

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
                if (_renderer != null)
                {
                    _renderer.material.SetColor("_Color", isPressured.Value ? Color.green : Color.red);    
                }
                if (_light != null)
                {
                    _light.color = isPressured.Value ? Color.green : Color.red;    
                }
                
            };
        }
    }
}
