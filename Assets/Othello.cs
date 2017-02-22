using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System;

public class Othello : MonoBehaviour
{

    public enum State : byte
    {
        Wall,
        Empty,
        Black,
        White
    }

    private enum Vec : int
    {
        Right = 1,
        RightUp = -8,
        RightDown = 10,
        Left = -1,
        LeftUp = -10,
        LeftDown = 8,
        Up = -9,
        Down = 9
    }

    public Piece piecePrefab;
    public Controller player1;
    public Controller player2;

    public State currentPlayer;

    private Piece[] board = new Piece[90];

    public void Start()
    {
        for (int i = 0; i < board.Length; i++)
        {
            float x = i % 9 - 1;
            float z = (x - i) / 9;

            State hand;
            
            if (i <= 8 || i % 9 == 0 || i >= 81)
            {
                hand = State.Wall;
            }
            else
            {
                hand = State.Empty;
            }

            board[i] = (Piece)Instantiate(piecePrefab, new Vector3(x+0.5f, -0.3f, z+0.5f), Quaternion.identity);
            board[i].Initialize(hand);
        }

        Initialize();
    }

    private void Initialize()
    {
        UpdateBoard(9 * 4 + 4, State.White);
        UpdateBoard(9 * 5 + 5, State.White);
        UpdateBoard(9 * 4 + 5, State.Black);
        UpdateBoard(9 * 5 + 4, State.Black);

        currentPlayer = State.Black;
    }

    public void PutPiece(int z, int x, State hand)
    {
        int row = -z;
        int column = x + 1;
        int index = 9 * row + column;
        if (row <= 8 && row >= 1
            && column <= 8 && column >= 1
            && board[index].state == State.Empty
            && CanPutPiece(index, hand)
          )
        {
            UpdateBoard(index, hand);

            if (IsBoardFill() == true)
            {
                int numBlack = board.Where(piece => piece.state == State.Black).Count();
                int numWhite = board.Where(piece => piece.state == State.White).Count();

                Debug.Log(String.Format("Black: {0}, White: {1}", numBlack, numWhite));
            }
            else
            {
                currentPlayer = (currentPlayer == State.Black) ? State.White : State.Black;
            }
        }
    }

    private bool IsBoardFill()
    {
        return !board.Any(x => x.state == State.Empty);
    }

    private void UpdateBoard(int index, State hand)
    {
        board[index].Initialize(hand);

        FlipPieces(index, hand);
    }

    private void FlipPieces(int index, State hand)
    {
        State opposite = (hand == State.Black) ? State.White : State.Black;

        foreach (int vec in Enum.GetValues(typeof(Vec)))
        {
            int cursor = index + vec;
            while (board[cursor].state == opposite)
            {
                cursor += vec;
            }

            if (board[cursor].state != hand)
            {
                continue;
            }

            cursor -= vec;
            while (cursor != index)
            {
                board[cursor].Flip();
                cursor -= vec;
            }
        }
    }

    private bool CanPutPiece(int index, State hand)
    {
        State opposite = (hand == State.Black) ? State.White : State.Black;

        foreach (int vec in Enum.GetValues(typeof(Vec)))
        {
            if (index + vec < 90)
            {
                if (board[index + vec].state != opposite)
                {
                    continue;
                }

                int times = 2;
                while (board[index + vec * times].state == opposite)
                {
                    times += 1;
                }

                if (board[index + vec * times].state == hand)
                {
                    return true;
                }
            }
        }

        return false;
    }

}
