using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fractal : MonoBehaviour {

	public Mesh mesh;
	public Material material;

	public int maxDepth;

	public float childScale;

	int depth;

	static Vector3[] childDirections = {
		Vector3.up,
		Vector3.forward,
		Vector3.back,
		Vector3.down,
		Vector3.left,
		Vector3.right
	};

	static Quaternion[] childQuaternion = {
		Quaternion.identity,
		Quaternion.Euler (90f, 0, 0),
		Quaternion.Euler(-90f, 0, 0),
		Quaternion.Euler(180f, 0, 0),
		Quaternion.Euler(0, 0, 90f),
		Quaternion.Euler(0, 0, -90f),
	};

	Material[] materials;

	void initMaterials()
	{
		materials = new Material[maxDepth + 1];
		for (int i = 0; i < materials.Length; ++i)
		{
			materials [i] = new Material (material);
			materials[i].color = Color.Lerp (Color.white, Color.yellow, (float)i / maxDepth);
		}
	}

	// Use this for initialization
	void Start () {
		if (materials == null)
		{
			initMaterials ();
		}
		gameObject.AddComponent<MeshFilter> ().mesh = mesh;
		gameObject.AddComponent<MeshRenderer> ().material = materials [depth];
		if (depth < maxDepth) 
		{
			StartCoroutine (CreateChildern());
		}
//		disableBig ();
	}

	void disableBig()
	{
		if (depth < (maxDepth / 2 + 1))
		{
			GetComponent<MeshRenderer> ().enabled = false;
		}
	}

	IEnumerator CreateChildern()
	{
		for (int i = 0; i < childDirections.Length; ++i)
		{
			yield return new WaitForSeconds (Random.Range(0.1f, 0.5f));
			new GameObject ("Fracal").AddComponent<Fractal> ().Initialize (this, childDirections [i], childQuaternion [i]);
		}
	}

	void Initialize(Fractal parent, Vector3 direction, Quaternion quaternion)
	{
		mesh = parent.mesh;
		materials = parent.materials;
		depth = parent.depth + 1;
		maxDepth = parent.maxDepth;
		childScale = parent.childScale;
		transform.parent = parent.transform;
		transform.localScale = Vector3.one * parent.childScale;
		transform.localPosition = direction * 0.5f * (1 + childScale);
		transform.localRotation = quaternion;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
