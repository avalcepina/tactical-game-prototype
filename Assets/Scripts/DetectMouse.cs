using UnityEngine;

namespace SA
{

    public class DetectMouse : ITurnState
    {
        public ITurnState execute()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 1000))
                {


                }

            }

            return this;

        }
    }


}