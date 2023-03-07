using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateKeeper : MonoBehaviour {
	// следующие константы зависят от файла изображения по умолчанию DelverTiles
	// если вы переупорядочите спрайты в DelverTiles, возможно, вам придётся изменить эти константы!!!
	// -------- Индексы плиток с запертыми дверьми
	const int lockedR = 73;
	const int lockedUR = 57;
	const int lockedUL = 56;
	const int lockedL = 72;
	const int lockedDL = 88;
	const int lockedDR = 89;

	// -------- Индексы плиток с открытыми дверьми
	const int openR = 70;
	const int openUR = 53;
	const int openUL = 52;
	const int openL = 67;
	const int openDL = 84;
	const int openDR = 85;

	private IKeyMaster keys;

	void Awake() {
		keys = GetComponent<IKeyMaster>();
	}
	
	void OnCollisionStay( Collision coll ) {
		// если ключей нет, можно не продолжать
		if (keys.keyCount < 1) return;

		// интерес представляют только плитки
		Tile ti = coll.gameObject.GetComponent<Tile>();
		if (ti == null) return;

		// открывать, только если Дрей обращён лицом к двери (предотвратить случайное использование ключа)
		int facing = keys.GetFacing();
		// проверить, является ли плитка закрытой дверью
		Tile ti2;
		switch (ti.tileNum) {
			case lockedR:
				if (facing != 0) return;
				ti.SetTile (ti.x, ti.y, openR);
				break;

			case lockedUR:
				if (facing != 1) return;
				ti.SetTile (ti.x, ti.y, openUR);
				ti2 = TileCamera.TILES [ti.x-1, ti.y];
				ti2.SetTile (ti2.x, ti2.y, openUL);
				break;

			case lockedUL:
				if (facing != 1) return;
				ti.SetTile (ti.x, ti.y, openUL);
				ti2 = TileCamera.TILES [ti.x+1, ti.y];
				ti2.SetTile (ti2.x, ti2.y, openUR);
				break;

			case lockedL:
				if (facing != 2) return;
				ti.SetTile (ti.x, ti.y, openL);
				break;

			case lockedDL:
				if (facing != 3) return;
				ti.SetTile (ti.x, ti.y, openDL);
				ti2 = TileCamera.TILES [ti.x+1, ti.y];
				ti2.SetTile (ti2.x, ti2.y, openDR);
				break;

			case lockedDR:
				if (facing != 3) return;
				ti.SetTile (ti.x, ti.y, openDR);
				ti2 = TileCamera.TILES [ti.x-1, ti.y];
				ti2.SetTile (ti2.x, ti2.y, openDL);
				break;

			default:
				return; // выйти, чтобы исключить уменьшение счётчика ключей
		}
		keys.keyCount--;
	}
}
