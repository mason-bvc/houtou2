using UnityEngine;

// Note: not implementing a true singleton because I deemed it unnecessary.

public class Game : MonoBehaviour
{
    public static AssetBundle AssetBundle { get; protected set; }
    public static Player Player { get; protected set; }

    // Rationale: When you put the responsibility on either object to delete
    // itself based on the others' tag in their own scripts, you do two things.
    // Number one, you rely on the execution order of the scripts of the
    // respective game objects to handle deleting themselves in the proper
    // order.* Number two, you give the game objects a weak awareness of each
    // other, slightly increasing the coupling. Not only those, but you also
    // increase the number of contribution points and "jumping around" you have
    // to do in the codebase in order to change the deletion behavior. That all
    // majorly sucks.

    // Because this project is due very soon, rapid development concerns start
    // to pile up quick. If this was an honest-to-God piece of software that
    // was managed by a medium-large company that does sprints, kanbas, scrums,
    // and all that crap, I would probably add a
    // "TriggerEnter2DBehaviorProvider" that acts as somewhat of a Command
    // Pattern Factory that itself decides what should be done when two
    // colliders of two different tags hit each other in order to divide
    // responsibilities, but I really do not have time or desire to prematurely
    // abstract and architect like I'm a Gang of Four enthusiast from the
    // mid-'90s.

    // So in the end, we get a monolithic Game class, but we also get
    // some dependency inversion and _some_ decoupling, at least between the
    // two colliding objects. The objects themselves must still be coupled with
    // this Game manager object since they have to directly call into its
    // static fields because, again, time concerns. This can be alleviated with
    // some simple dependency injection, but as far as I know, there is no
    // event you can hook into that fires when a Transform is added to a scene
    // wherein to "Inject" your "Dependencies" through a hypothetical
    // higher-order object.

    // * you could defer the deletion of the objects to the end of the frame,
    // but you still run into problem number two.

    public static void HandleTriggerEnter2D(Collider2D collider1, Collider2D collider2)
    {
        if (collider1.CompareTag(Global.TAG_ENEMY) && collider2.CompareTag(Global.TAG_PLAYER_BULLET))
        {
            Destroy(collider1.transform.root.gameObject);
            Destroy(collider2.transform.root.gameObject);
        }
    }

    protected void Start()
    {
        AssetBundle = AssetBundle.LoadFromFile("Assets/AssetBundles/assetbundle");
        Player = FindObjectOfType<Player>();
    }
}
