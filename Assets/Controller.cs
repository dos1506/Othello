using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {

    public Othello manager;
    public Othello.State hand;

    public void Update()
    {
        if (Input.GetButtonDown("Fire1") && manager.currentPlayer == hand)
        {
            Vector3 pointOnBoard = GetPointOnBoard(Input.mousePosition);
            float column = Mathf.Floor(pointOnBoard.x);
            float row = Mathf.Floor(pointOnBoard.z);
            manager.PutPiece((int)row, (int)column, hand);
        }
    }

    private Vector3 GetPointOnBoard(Vector3 screenPoint)
    {
        Camera camera = GetComponent<Camera>();
        var tmpPoint = new Vector3(screenPoint.x, screenPoint.y, 10f);  
        Vector3 worldPoint = camera.ScreenToWorldPoint(tmpPoint);

        return worldPoint;
    }

}
