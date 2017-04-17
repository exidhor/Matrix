using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Matrix
{
    public class Shield : Effect
    {
        private AnimatorComponent _animatorComponent;

        void Awake()
        {
            _animatorComponent = GetComponent<AnimatorComponent>();
        }

        protected override void Update()
        {
            if (_currentLifeTime == 0)
            {
                if (_animatorComponent != null)
                {
                    _animatorComponent.SetCurrentAnimation("default");
                }
            }

            base.Update();
        }
    }
}
