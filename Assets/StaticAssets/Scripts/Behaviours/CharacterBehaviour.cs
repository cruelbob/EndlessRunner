using UnityEngine;
using System.Collections;
using System;

public enum Line {
    Left,
    Center,
    Right
}

public class CharacterBehaviour : MonoBehaviour {
    public event Action<GameObject> OnWall;

    private const float SPEED = 5f;
    private const float CHANGE_LINE_TIME = 0.25f;
    private const float JUMP_HEIGHT = 0.75f;
    private const float JUMP_TIME = 0.5f;
    private const float LINE_WIDTH = 1f;

    public GameObject Camera;
    public Animator Animator;
    private bool changingLine;
    private bool turning;
    private CardinalDirection cardinalDirection;
    private Line line;

    public void Init() {
        Animator.SetFloat("Forward", 1f);
        line = Line.Center;
    }

    public void Stop() {
        Animator.SetFloat("Forward", 0f);
        enabled = false;
    }

    public void MoveLeft() {
        if(changingLine || !enabled) {
            return;
        }
        CardinalDirection nextCardinalDirection = MainController.Instance.LevelController.Level.NextPartDirection;
        if (LevelBehaviour.GetDirection(cardinalDirection, nextCardinalDirection) == Direction.Left && nextCardinalDirection != cardinalDirection) {
            transform.Rotate(0, -90f, 0);
            cardinalDirection = nextCardinalDirection;
            SetCorrectPositionForNextPart();
        } else if (line != Line.Left) {
            StartCoroutine(ChangeLine(line - 1));
        }
    }

    public void MoveRight() {
        if (changingLine || !enabled) {
            return;
        }
        CardinalDirection nextCardinalDirection = MainController.Instance.LevelController.Level.NextPartDirection;
        if (LevelBehaviour.GetDirection(cardinalDirection, nextCardinalDirection) == Direction.Right && nextCardinalDirection != cardinalDirection) {
            transform.Rotate(0, 90f, 0);
            cardinalDirection = nextCardinalDirection;
            SetCorrectPositionForNextPart();
        } else if (line != Line.Right) {
            StartCoroutine(ChangeLine(line + 1));
        }
    }

    public void Jump() {
        if(!enabled) {
            return;
        }

        StartCoroutine(JumpCoroutine());
    }

    private IEnumerator ChangeLine(Line newLine) {
        changingLine = true;

        Vector3 startPos = transform.localPosition;
        if (cardinalDirection == CardinalDirection.North) {
            float xOffset = newLine > line ? LINE_WIDTH : -LINE_WIDTH;
            while (Mathf.Abs(transform.localPosition.x - startPos.x) < Mathf.Abs(xOffset)) {
                transform.localPosition += new Vector3(Mathf.Lerp(0, xOffset, Time.fixedDeltaTime / CHANGE_LINE_TIME), 0, 0);
                yield return new WaitForFixedUpdate();
            }
            transform.localPosition = new Vector3(startPos.x + xOffset, transform.localPosition.y, transform.localPosition.z);
        } else {
            float zOffset = newLine > line ? LINE_WIDTH : -LINE_WIDTH;
            zOffset *= cardinalDirection == CardinalDirection.East ? 1f : -1f;
            while (Mathf.Abs(transform.localPosition.z - startPos.z) < Mathf.Abs(zOffset)) {
                transform.localPosition += new Vector3(0, 0, Mathf.Lerp(0, zOffset, Time.fixedDeltaTime / CHANGE_LINE_TIME));
                yield return new WaitForFixedUpdate();
            }
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, startPos.z + zOffset);
        }

        line = newLine;
        changingLine = false;
    }

    private IEnumerator JumpCoroutine() {
        Animator.SetBool("OnGround", false);
        float time = 0;
        while(time < JUMP_TIME) {
            var pos = transform.localPosition;
            pos.y = Mathf.Sin((time / JUMP_TIME) * Mathf.PI) * JUMP_HEIGHT;
            transform.localPosition = pos;
            yield return new WaitForFixedUpdate();
            time += Time.fixedDeltaTime;
        }
        transform.localPosition = new Vector3(transform.localPosition.x, 0, transform.localPosition.z);
        Animator.SetBool("OnGround", true);
    }

    private void Update() {
        transform.position += transform.forward * Time.fixedDeltaTime * SPEED;
    }

    private void SetCorrectPositionForNextPart() {
        GameObject nextPart = MainController.Instance.LevelController.Level.NextCharacterPart;
        Vector3 correctPosition = transform.position;
        switch(MainController.Instance.LevelController.Level.NextPartDirection) {
            case CardinalDirection.North:
                switch(line) {
                    case Line.Center:
                        correctPosition.x = nextPart.transform.position.x;
                        break;
                    case Line.Left:
                        correctPosition.x = nextPart.transform.position.x - LINE_WIDTH;
                        break;
                    case Line.Right:
                        correctPosition.x = nextPart.transform.position.x + LINE_WIDTH;
                        break;
                }
                break;
            case CardinalDirection.East:
                switch (line) {
                    case Line.Center:
                        correctPosition.z = nextPart.transform.position.z;
                        break;
                    case Line.Left:
                        correctPosition.z = nextPart.transform.position.z - LINE_WIDTH;
                        break;
                    case Line.Right:
                        correctPosition.z = nextPart.transform.position.z + LINE_WIDTH;
                        break;
                }
                break;
            case CardinalDirection.West:
                switch (line) {
                    case Line.Center:
                        correctPosition.z = nextPart.transform.position.z;
                        break;
                    case Line.Left:
                        correctPosition.z = nextPart.transform.position.z + LINE_WIDTH;
                        break;
                    case Line.Right:
                        correctPosition.z = nextPart.transform.position.z - LINE_WIDTH;
                        break;
                }
                break;
        }
        transform.position = correctPosition;
    }

    private void OnTriggerEnter(Collider other) {
        if(other.name == "Wall") {
            if(OnWall != null) {
                OnWall(other.gameObject);
            }
        }
    }
}
