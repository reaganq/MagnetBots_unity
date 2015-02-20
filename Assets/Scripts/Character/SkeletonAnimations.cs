using UnityEngine;
using System.Collections;

public class SkeletonAnimations : MonoBehaviour {

	public ArmorAnimation[] animations;
	public int playid;
	public bool autoPlay;
	// Use this for initialization
	void Start () {
		foreach (var item in animations) {
			animation.AddClip(item.clip, item.clip.name);
			animation[item.clip.name].layer = item.animationLayer;
				}
		if(autoPlay)
		animation.Play(animations[playid].clip.name);
	}
}
