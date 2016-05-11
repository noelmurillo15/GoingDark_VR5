using UnityEngine;

namespace AGS.Core.Examples.ActionPropertiesExamples
{
    public class ActionPropertyExampleView : MonoBehaviour
    {
        public ActionPropertyExampleModel MyModelInstance;

        void Awake()
        {
            MyModelInstance = new ActionPropertyExampleModel();
            MyModelInstance.MySubscribableInt.OnValueChanged += (sender, intVal) =>
            {
                // Write changed value to the debug log
                Debug.Log(intVal.Value);
            };
        }

        void OnEnable()
        {
            MyModelInstance.StartTimer();
        }

        void OnDisable()
        {
            MyModelInstance.StopTimer();
        }
    }
}
