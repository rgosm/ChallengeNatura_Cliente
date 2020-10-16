package crc645255ab2e60a8ccd8;


public class DetectShake
	extends android.support.v7.app.AppCompatActivity
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("AppNaturaCliente.DetectShake, AppNaturaCliente", DetectShake.class, __md_methods);
	}


	public DetectShake ()
	{
		super ();
		if (getClass () == DetectShake.class)
			mono.android.TypeManager.Activate ("AppNaturaCliente.DetectShake, AppNaturaCliente", "", this, new java.lang.Object[] {  });
	}

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
