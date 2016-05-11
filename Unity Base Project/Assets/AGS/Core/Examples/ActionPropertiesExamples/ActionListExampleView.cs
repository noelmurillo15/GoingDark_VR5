using UnityEngine;

namespace AGS.Core.Examples.ActionPropertiesExamples
{
    public class ActionListExampleView : MonoBehaviour
    {
        public ActionListExampleModel MyModelInstance = new ActionListExampleModel();

        void Awake()
        {
            MyModelInstance.MySubscribableList.ListItemAdded += listItem =>
            {
                // Write new listItem to the debug log
                Debug.Log(listItem);
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
