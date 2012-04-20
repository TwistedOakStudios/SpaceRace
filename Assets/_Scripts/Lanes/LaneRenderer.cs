using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class LaneRenderer : MonoBehaviour {
    public GameObject prefab;
    const int width = 256, height = 256;
    const int padding = 8;
    const int size = 32;
    const int cols = 6;
    Queue<GameObject> tiles = new Queue<GameObject>();
    public int length = 20;

    //void Start() {
    //    AddTiles(3, 3, 1, 1, -15 - 3);
    //    AddTiles(2, 3, 2, 1, -15 - 2);
    //    AddTiles(1, 2, 3, 2, -15 - 1);
    //    AddTiles(1, 1, 3, 3, -15 + 0);
    //    AddTiles(1, 1, 3, 3, -15 + 1);
    //    AddTiles(0, 1, 2, 3, -15 + 2);
    //    AddTiles(0, 0, 2, 2, -15 + 3);
    //    AddTiles(1, 0, 3, 2, -15 + 4);
    //    AddTiles(1, 1, 3, 3, -15 + 5);
    //    AddTiles(2, 1, 3, 3, -15 + 6);
    //    AddTiles(2, 2, 3, 3, -15 + 7);
    //    AddTiles(3, 2, 2, 3, -15 + 8);
    //    AddTiles(3, 3, 2, 2, -15 + 9);
    //    AddTiles(2, 3, 3, 2, -15 + 10);
    //    AddTiles(2, 2, 3, 3, -15 + 11);
    //    AddTiles(3, 2, 3, 3, -15 + 12);
    //    AddTiles(3, 3, 3, 3, -15 + 13);
    //    AddTiles(2, 3, 2, 3, -15 + 14);
    //    AddTiles(2, 2, 2, 2, -15 + 15);
    //    AddTiles(1, 2, 3, 2, -15 + 16);
    //    AddTiles(1, 1, 3, 3, -15 + 17);
    //    AddTiles(2, 1, 2, 3, -15 + 18);
    //    AddTiles(2, 2, 2, 2, -15 + 19);
    //    AddTiles(3, 2, 2, 2, -15 + 20);
    //    AddTiles(3, 3, 2, 2, -15 + 21);
    //    AddTiles(2, 3, 2, 2, -15 + 22);
    //    AddTiles(1, 2, 2, 2, -15 + 23);
    //    AddTiles(1, 1, 2, 2, -15 + 24);
    //    AddTiles(2, 1, 2, 2, -15 + 25);
    //    AddTiles(2, 2, 3, 2, -15 + 26);
    //    AddTiles(2, 2, 3, 3, -15 + 27);
    //    AddTiles(2, 2, 2, 3, -15 + 28);
    //    AddTiles(2, 2, 1, 2, -15 + 29);
    //    AddTiles(2, 2, 1, 1, -15 + 30);
    //    AddTiles(2, 2, 2, 1, -15 + 31);
    //    AddTiles(2, 2, 2, 2, -15 + 32);
    //}


    IEnumerator Start() {
        int lastBlue = -1, lastRed = 1;
        PushTiles(lastBlue, lastBlue, lastRed, lastRed);
        while (true) {
            var blue = lastBlue + Mathf.RoundToInt(Random.value * 3 - 1.5f);
            var red = lastRed + Mathf.RoundToInt(Random.value * 3 - 1.5f);
            red = Mathf.Clamp(red, -2, 2);
            blue = Mathf.Clamp(blue, -2, 2);
            PushTiles(blue, lastBlue, red, lastRed);
            lastBlue = blue;
            lastRed = red;
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void PushTiles(int blue, int lastBlue, int red, int lastRed) {
        foreach (var go in tiles) {
            var pos = go.transform.localPosition;
            pos.x -= 1;
            go.transform.localPosition = pos;
        }

        while (tiles.Count > 0 && tiles.Peek().transform.localPosition.x < -length) Destroy(tiles.Dequeue());
        AddTiles(blue, lastBlue, red, lastRed, 0);
    }
    void AddTiles(int blue, int lastBlue, int red, int lastRed, int distance) {
        Tile a = Tile.None, b = Tile.None, c = Tile.None, d = Tile.None;

        var fromGold = lastRed == lastBlue;
        var toGold = red == blue;
        var swapped = red == lastBlue && blue == lastRed;
        var solidGold = fromGold && toGold;
        var anyGold = fromGold || toGold || swapped;
        var blueChanged = blue != lastBlue;
        var redChanged = red != lastRed;

        if (!anyGold) {
            if (redChanged) {
                a = red > lastRed ? Tile.RedFD : Tile.RedFU;
                c = red > lastRed ? Tile.RedTU : Tile.RedTD;
            }
            else {
                a = Tile.Red;
            }
            if (blueChanged) {
                b = blue > lastBlue ? Tile.BlueFD : Tile.BlueFU;
                d = blue > lastBlue ? Tile.BlueTU : Tile.BlueTD;
            }
            else {
                b = Tile.Blue;
            }

            goto end;
        }

        if (solidGold) {
            if (red == lastRed) {
                a = Tile.Gold;
                goto end;
            }
            a = red > lastRed ? Tile.GoldFD : Tile.GoldFU;
            c = red > lastRed ? Tile.GoldTU : Tile.GoldTD;
            goto end;
        }

        if (swapped) {
            if (red > blue) {
                a = Tile.BRD;
                b = Tile.RBU;
            }
            else {
                a = Tile.BRU;
                b = Tile.RBD;
            }
            goto end;
        }

        if (toGold) {
            if (redChanged) {
                c = red > lastRed ? Tile.RedTU : Tile.RedTD;
                if (blueChanged) {
                    a = red > lastRed ? Tile.GoldBRI : Tile.GoldRBI;
                }
                else {
                    a = red > lastRed ? Tile.GoldBU : Tile.GoldBD;
                }
            }
            if (blueChanged) {
                d = blue > lastBlue ? Tile.BlueTU : Tile.BlueTD;
                if (!redChanged) {
                    b = blue > lastBlue ? Tile.GoldRU : Tile.GoldRD;
                }
            }
            goto end;
        }

        if (fromGold) {
            if (redChanged) {
                a = red > lastRed ? Tile.RedFD : Tile.RedFU;
                if (blueChanged) {
                    c = red < lastRed ? Tile.GoldBRO : Tile.GoldRBO;
                }
                else {
                    c = red < lastRed ? Tile.GoldUB : Tile.GoldDB;
                }
            }
            if (blueChanged) {
                b = blue > lastBlue ? Tile.BlueFD : Tile.BlueFU;
                if (!redChanged) {
                    d = blue < lastBlue ? Tile.GoldUR : Tile.GoldDR;
                }
            }
            goto end;
        }

        Debug.LogError(string.Format("missing case: {0}, {1}, {2}, {3}", blue, lastBlue, red, lastRed));

    end:
        MakeTile(a, distance, red);
        MakeTile(b, distance, blue);
        MakeTile(c, distance, lastRed);
        MakeTile(d, distance, lastBlue);
        return;
    }

    GameObject MakeTile(Tile type, float x, float y) {
        if (type == Tile.None) return null;
        var go = Instantiate(prefab) as GameObject;
        tiles.Enqueue(go);
        var t = go.transform;
        t.parent = transform;
        t.localPosition = new Vector3(x, -y);
        t.localRotation = Quaternion.identity;

        var r = go.GetComponentInChildren<Renderer>();
        var m = r.material;
        var row = (int)type / cols;
        var col = (int)type % cols;
        m.mainTextureOffset = new Vector2(col * (padding + size) /(float) width,1f- row * (padding + size) / (float)height);
        m.mainTextureScale = new Vector2(size / (float)width, -size / (float)height);
        return go;
    }
}

public enum Tile {
    None = -1,
    Blue   = 0,
    BlueTD = 1,
    BlueTU = 2,
    BlueFU = 3,
    BlueFD = 4,

    Red    = 5,
    RedTD  = 6,
    RedTU  = 7,
    RedFU  = 8,
    RedFD  = 9,
    
    Gold    = 10,
    GoldTD  = 11,
    GoldTU  = 12,
    GoldFU  = 13,
    GoldFD  = 14,

    GoldBRI = 15,
    GoldRBI = 20,
    GoldBRO = 29,
    GoldRBO = 30,

    GoldRD = 16,
    GoldRU = 17,
    GoldDR = 18,
    GoldUR = 19,

    GoldBD = 21,
    GoldBU = 22,
    GoldDB = 23,
    GoldUB = 24,

    RBD = 25,
    BRU = 26,
    BRD = 27,
    RBU = 28,
}

