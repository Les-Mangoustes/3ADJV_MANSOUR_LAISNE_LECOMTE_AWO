using UnityEngine;
using System.Collections;

public static class checkPosition{

    public static int _rangeX = Mathf.FloorToInt(GameObject.Find("Plane").renderer.bounds.size.x)/2;
    public static int _rangeZ = Mathf.FloorToInt(GameObject.Find("Plane").renderer.bounds.size.z)/2;

    /*public static Vector3 addRandomPos()
    {
        Debug.Log("rand pos");
        bool _otherCollider = true;
        Vector3 pos = Vector3.zero;
        while (_otherCollider)
        {
            _otherCollider = false;
            pos = new Vector3(Random.Range(-_rangeX, _rangeX), 0, Random.Range(-_rangeZ, _rangeZ));
            Collider[] hitColliders = Physics.OverlapSphere(pos, 2);
            foreach(Collider col in hitColliders)
            {
                if (col.name != "Plane")
                {
                    _otherCollider = true;
                }
            }
        }
        pos.y = 1;
        return pos;
    }*/

    public static Vector3 addRandomPos()
    {

        Transform refPlane = GameObject.Find("Plane").transform;
        bool _isGoodPos = false;
        Vector3 pos = Vector3.zero;
        while (!_isGoodPos)
        {
            //_isGoodPos = true;
            pos = new Vector3(Random.Range(-_rangeX, _rangeX), 0, Random.Range(-_rangeZ, _rangeZ));
            var all = Physics.OverlapSphere(pos, 3f);
            if (all.Length == 1 && all[0].transform == refPlane)
            {
                //Debug.DrawRay(pos, Vector3.down * 100,Color.cyan, 10);   
                _isGoodPos = true;
            }

        }
        pos.y = 1;
        return pos;
    }
}
