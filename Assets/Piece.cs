using UnityEngine;
using System.Collections;
using UniRx;
using System;

public class Piece : MonoBehaviour
{
    public Othello.State state;

    public void Flip()
    {
        int flipFrame = 30;
        int sign;
        int rotationScale = 180 / flipFrame;

        Observable.EveryUpdate()
                  .Take(flipFrame)
                  .Subscribe(x =>
                  {
                      // コマの回転
                      transform.Rotate(rotationScale, 0f, 0f);

                      // コマの上下
                      Vector3 pos = transform.position;
                      sign = (x >= flipFrame / 2) ? -1 : 1;
                      transform.position = new Vector3(pos.x, pos.y + sign * 0.1f, pos.z);
                  });
        // 白黒反転
        if (state == Othello.State.Black)
        {
            state = Othello.State.White;
        }
        else
        {
            state = Othello.State.Black;
        }
    }

    public void Initialize(Othello.State hand)
    {
        state = hand; 
        if (state == Othello.State.White)
        {
            gameObject.SetActive(true);
            transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
        }
        else if (state == Othello.State.Black)
        {
            gameObject.SetActive(true);
            transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
