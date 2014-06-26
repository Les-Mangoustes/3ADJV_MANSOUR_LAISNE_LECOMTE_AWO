using UnityEngine;
using System.Collections;

public class AStarScriptTutorial {

    private NetworkView networkView;

    private Vector3 targetPos;

    private Vector3[] pathToTargetCorner;

    private int currentIndexCheckPoint = 1;
    private int endIndexCheckPoint = 0;
    private float minimalDist = 0.01f;
    public float minimalDistSet
    {
        set { minimalDist = value; }
    }


    public void setTarget(Vector3 startPos,Vector3 newTarget)
    {
        if (Network.isServer)
        {
            targetPos = newTarget;
            NavMeshPath pathToTarget;
            if ((pathToTarget = getCalcPath(startPos, targetPos)) != null)
            {
                pathToTargetCorner = pathToTarget.corners;
                this.currentIndexCheckPoint = 1;
                this.endIndexCheckPoint = pathToTargetCorner.Length;
            }
            string pathCorner = "";
            if (pathToTarget != null)
            {
                foreach (Vector3 corner in pathToTarget.corners)
                    pathCorner += corner.x.ToString() + "," + corner.y.ToString() + "," + corner.z.ToString() + ";";
                networkView.RPC("changeDirection", RPCMode.Others, pathCorner);
            }
        }
    }

    public Vector3 checkPath(Vector3 startPos, float speed)
    {

        if (endIndexCheckPoint > currentIndexCheckPoint)
        {
            Vector3 direction = pathToTargetCorner[currentIndexCheckPoint] - startPos;
            direction.y = 0;
            if (direction.sqrMagnitude < minimalDist)
            {
                currentIndexCheckPoint++;
                return checkPath(startPos, speed);
            }
            else
            {
                return direction.normalized * speed * Time.deltaTime;
            }
        }
        return Vector3.zero;
    }

    private NavMeshPath getCalcPath(Vector3 origin, Vector3 wantToGo)
    {
        NavMeshPath path = new NavMeshPath();
        NavMesh.CalculatePath(origin, wantToGo, -1, path);

        if (path.corners.Length > 1)
            return path;
        else
            return null;
    }

    /*
     HOW USE IT : ADD a handler rpc in your IA Class
     * EXAMPLE
     * 
     *  deplacementScript = new AStarScript(networkView);
     *  
     *  [RPC]
        private void changeDirection(string pathCorner)
        {
            deplacementScript.changeDirection(pathCorner);
        }  
     */


    public void changeDirection(string pathCorner)
    {
        string[] arrayCorner = pathCorner.Split(';');
        pathToTargetCorner = new Vector3[arrayCorner.Length-1];
        pathToTargetCorner[0] = Vector3.zero;
        for (int i = 1; i < arrayCorner.Length-1;i++ )
        {
            string[] cornerFloat = arrayCorner[i].Split(',');
            Vector3 cornerVector = new Vector3(float.Parse(cornerFloat[0]), float.Parse(cornerFloat[1]), float.Parse(cornerFloat[2]));
            pathToTargetCorner[i] = cornerVector;
        }
        currentIndexCheckPoint = 1;
        endIndexCheckPoint = pathToTargetCorner.Length;
    }
}
