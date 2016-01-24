using UnityEngine;

public class SingletonBehaviour<T> : MonoBehaviour where T : SingletonBehaviour<T> {
    public virtual void Awake() {
        SetInstance();
    }

    public virtual void OnDestroy() {
        m_instance = null;
    }

    public static T Instance {
        get {
            return m_instance;
        }
    }

//protected
    protected void SetInstance() {
        if (m_instance != null) {
            throw new System.Exception("Trying to create a second instance of a singleton: " + this.ToString());
        }

        m_instance = (T)this;
    }
    protected static T m_instance = null;
}

public class ConstructorAvailableSingletonBehaviour<T> : SingletonBehaviour<T> where T : ConstructorAvailableSingletonBehaviour<T> {
    public ConstructorAvailableSingletonBehaviour() {
        SetInstance();
    }

    public override void Awake() {}
}