using AGS.Core.Base;
using AGS.Core.Classes.ActionProperties;
using UnityEngine;

namespace AGS.Core.Systems.BaseSystem
{
    /// <summary>
    /// Base class for AGS ActionViews. After model depencies are solved (SolveModelDependencies method has run) they hold a reference to be base Model object.
    /// When the view has been initialized (InitializeActionModel method has run) it will set its ViewReady property to true.
    /// BaseView is also responsible for destroying the Model if view is destroyed in the Unity scene, or destroying itself if Model is destroyed
    /// </summary>
    public abstract class ActionView : MonoBehaviour
    {
        public ActionModel ActionModel;
        public ActionProperty<bool> ViewReady { get; private set; }

        #region MonoBehaviour

        public virtual void Awake()
        {
            ViewReady = new ActionProperty<bool>();
        }

        public virtual void Start()
        {
            // If this ActionView is not already known (and owned) by the GameManager, add this to its list of ActionViews. This makes it possible to add ActionViews dynamically
            if (!GameManager.ActionViews.Contains(this))
            {
                GameManager.ActionViews.Add(this);
            }
        }

        public virtual void Update()
        {

        }

        public virtual void FixedUpdate()
        {

        }

        public virtual void OnDestroy()
        {
            if (ActionModel != null)
            {
                ActionModel.DestroyModel();
            }

        }
        #endregion

        /// <summary>
        /// Initializes the view. This method is called by the GameManager on MonoBehaviour Start()
        /// Call the Views ActionModels constructor from here to create the model, then Call SolveModelDepencies with model as parameter
        /// </summary>
        public abstract void InitializeView();

        /// <summary>
        /// Solves the model dependencies. This method should called by the View after the ActionModel has been created
        /// </summary>
        /// <param name="model">The ActionModel that was instaniated in InitializeView.</param>
        public virtual void SolveModelDependencies(ActionModel model)
        {
            ActionModel = model;
        }

        /// <summary>
        /// Initializes the action model. This method is called by the GameManger, after depencies have been solved
        /// When called, all models have been created and all dependencies are solved.
        /// Subscribe to ActionModels properties here.
        /// Make sure to call base.InitializeActionModel(model) if overriding this method
        /// </summary>
        /// <param name="model">The model.</param>
        public virtual void InitializeActionModel(ActionModel model)
        {
            ViewReady.Value = true;
            ActionModel.ModelDestroyed += DestroyView;
        }

        /// <summary>
        /// Destroys the view.
        /// </summary>
        public void DestroyView()
        {
            Destroy(gameObject);
        }
    }
}
