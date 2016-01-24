using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class LevelBehaviour : MonoBehaviour {
    public const int PARTS_NUMBER = 10;

    public Action<int> OnNewResult;
    public Action<int> OnPointsChanged;

    public DefaultLevelPart DefaultPrefab;
    public GameObject CornerPrefab;
    public GameObject JumpPrefab;
    public GameObject MovePrefab;
    public CharacterBehaviour CharacterPrefab;

    public CharacterBehaviour Character { get; private set; }

    private LinkedList<GameObject> levelParts;
    private CardinalDirection cardinalDirection;
    private LinkedListNode<GameObject> characterPartNode;
    private int points;
    
    public bool Active { get; private set; }

    public int Points {
        get {
            return points;
        }
        private set {
            points = value;
            if (OnPointsChanged != null) {
                OnPointsChanged(points);
            }
        }
    }

    public CardinalDirection NextPartDirection {
        get {
            return GetCardinalDirectionByAngle(NextCharacterPart.transform.localEulerAngles.y);
        }
    }

    public CardinalDirection CurrentPartDirection {
        get {
            return GetCardinalDirectionByAngle(CurrentCharacterPart.transform.localEulerAngles.y);
        }
    }

    public GameObject CurrentCharacterPart {
        get {
            return characterPartNode.Value;
        }
    }

    public GameObject NextCharacterPart {
        get {
            return characterPartNode.Next.Value;
        }
    }

    public void Init() {
        levelParts = new LinkedList<GameObject>();
        DefaultLevelPart beginning = GameObject.Instantiate(DefaultPrefab);
        beginning.transform.SetParent(transform, false);
        levelParts.AddLast(beginning.gameObject);
        characterPartNode = levelParts.First;
        cardinalDirection = CardinalDirection.North;
        GenerateParts();

        Character = GameObject.Instantiate(CharacterPrefab);
        Character.transform.SetParent(transform, false);
        Character.transform.position = beginning.Spawn.position;
        Character.OnWall += OnWall;
        Character.Init();
        Active = true;
    }

    private void OnWall(GameObject wall) {
        wall.GetComponent<MeshRenderer>().material.color = Color.red;
        Character.Stop();
        Active = false;
        if (OnNewResult != null) {
            OnNewResult(Points);
        }
    }

    private void FixedUpdate() {
        var nextPartTransform = characterPartNode.Next.Value.transform;
        var nextPartRect = new Rect(nextPartTransform.localPosition.x - 1.5f, nextPartTransform.localPosition.z - 1.5f, 3, 5);
        var characterPos = new Vector2(Character.transform.localPosition.x, Character.transform.localPosition.z);
        if (nextPartRect.Contains(characterPos)) {
            characterPartNode = characterPartNode.Next;
            Destroy(levelParts.First.Value);
            levelParts.RemoveFirst();
            GenerateParts();
            Points++;
        }
    }

    private void GenerateParts() {
        while (levelParts.Count < PARTS_NUMBER) {
            GameObject nextpart;
            Direction newDirection = RandomDirection();
            if (newDirection != Direction.Forth) {
                nextpart = Instantiate(CornerPrefab);
                if (newDirection == Direction.Left) {
                    nextpart.transform.localScale = new Vector3(-1, 1, 1);
                }
            } else {
                nextpart = GetRandomPart();
            }
            nextpart.transform.Rotate(0, GetAngleByCardinalDirection(cardinalDirection), 0);
            Vector3 offset;
            switch (cardinalDirection) {
                case CardinalDirection.North:
                    offset = Vector3.forward;
                    break;
                case CardinalDirection.West:
                    offset = Vector3.right;
                    break;
                case CardinalDirection.East:
                    offset = Vector3.left;
                    break;
                default:
                    offset = Vector3.zero;
                    break;
            }
            offset *= 5f; // level part size
            nextpart.transform.localPosition = levelParts.Last().transform.localPosition + offset;
            nextpart.transform.SetParent(transform, false);
            levelParts.AddLast(nextpart);
            cardinalDirection = GetNewCardinalDirection(newDirection);
        }
    }

    private GameObject GetRandomPart() {
        float value = UnityEngine.Random.value;
        if(value < 0.1f) {
            return Instantiate(DefaultPrefab).gameObject;
        } else if (value < 0.55f) {
            return Instantiate(JumpPrefab);
        } else {
            return Instantiate(MovePrefab);
        }
    }

    private CardinalDirection GetNewCardinalDirection(Direction direction) {
        return GetNewCardinalDirection(cardinalDirection, direction);
    }

    public static CardinalDirection GetNewCardinalDirection(CardinalDirection cardinalDirection, Direction direction) {
        return (CardinalDirection)(((int)cardinalDirection + (int)direction) % 4);
    }

    public static Direction GetDirection(CardinalDirection current, CardinalDirection next) {
        return (Direction)((4 + ((int)next - (int)current)) % 4);
    }

    public static float GetAngleByCardinalDirection(CardinalDirection cardinalDirection) {
        return (int)cardinalDirection * 90f;
    }

    public static CardinalDirection GetCardinalDirectionByAngle(float angle) {
        return (CardinalDirection)((int)angle / 90);
    }

    private Direction RandomDirection() {
        float value = UnityEngine.Random.value;
        switch (cardinalDirection) { // we dont need to check parts collisions if we disallow South direction
            case CardinalDirection.North:
                if (value < 0.66f) {
                    return Direction.Forth;
                } else if (value < 0.82f) {
                    return Direction.Right;
                } else {
                    return Direction.Left;
                }
            case CardinalDirection.West:
                if (value > 0.66f) {
                    return Direction.Forth;
                } else {
                    return Direction.Left;
                }
            case CardinalDirection.East:
                if (value > 0.66f) {
                    return Direction.Forth;
                } else {
                    return Direction.Right;
                }
        }
        return Direction.Forth;
    }
}