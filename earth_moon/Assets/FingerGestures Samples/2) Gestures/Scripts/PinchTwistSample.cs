using UnityEngine;
using System.Collections;

/// <summary>
/// This sample demonstrates how to use the two-fingers Pinch and Twist gesture events to control the scale and orientation of a rectangle on the screen
/// </summary>
public class PinchTwistSample : SampleBase
{
    public enum InputMode
    {
        PinchOnly,
        TwistOnly,
        PinchAndTwist
    }

    public Transform target;
    public Material twistMaterial;
    public Material pinchMaterial;
    public Material pinchAndTwistMaterial;
    public float pinchScaleFactor = 0.02f;

    bool rotating = false;
    bool pinching = false;
    Material originalMaterial;
    
    bool Rotating
    {
        get { return rotating; }
        set
        {
            if( rotating != value )
            {
                rotating = value;
               
            }
        }
    }

    bool Pinching
    {
        get { return pinching; }
        set
        {
            if( pinching != value )
            {
                pinching = value;
                
            }
        }
    }
    
    #region FingerGestures Messages

    void OnTwist( TwistGesture gesture )
    {
        if( gesture.Phase == ContinuousGesturePhase.Started )
        {
            Rotating = true;
        }
        else if( gesture.Phase == ContinuousGesturePhase.Updated )
        {
            if( Rotating )
            {

                // apply a rotation around the Z axis by rotationAngleDelta degrees on our target object
                target.Rotate( 0, 0, gesture.DeltaRotation );
            }
        }
        else
        {
            if( Rotating )
            {
                Rotating = false;
            }
        }
    }

    void OnPinch( PinchGesture gesture )
    {
        if( gesture.Phase == ContinuousGesturePhase.Started )
        {
            Pinching = true;
        }
        else if( gesture.Phase == ContinuousGesturePhase.Updated )
        {
            if( Pinching )
            {
                // change the scale of the target based on the pinch delta value
                target.transform.position += new Vector3(0,0,gesture.Delta.Centimeters() * pinchScaleFactor);
            }
        }
        else
        {
            if( Pinching )
            {
                Pinching = false;
            }
        }
    }

    #endregion

    #region Misc

//    void UpdateTargetMaterial()
//    {
//        Material m;
//
//        if( pinching && rotating )
//            m = pinchAndTwistMaterial;
//        else if( pinching )
//            m = pinchMaterial;
//        else if( rotating )
//            m = twistMaterial;
//        else
//            m = originalMaterial;
//
//        target.renderer.sharedMaterial = m;
//    }

    #endregion

    #region Setup

    
    protected override void Start()
    {
        base.Start();

       
 
    }

    #endregion

}
