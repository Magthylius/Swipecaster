using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class EnvironmentFader : MonoBehaviour
{
    enum FaderMode
    {
        FADE_OPAQUE = 0,
        FADE_TRANSPARENT,
        PING_PONG
    }

    public float transitionSpeed;

    bool allowTransition = false;
    bool pingPongBool = false;

    Image image;
    FaderMode mode = FaderMode.PING_PONG;

    Color opaque;
    Color transparent;

    void Start()
    {
        image = GetComponent<Image>();
        opaque = image.color;
        transparent = opaque;

        opaque.a = 1f;
        transparent.a = 0f;
    }

    void Update()
    {
        if (allowTransition)
        {
            if (mode == FaderMode.FADE_OPAQUE)
            {
                image.color = Color.Lerp(image.color, opaque, transitionSpeed * Time.unscaledDeltaTime);

                if (opaque.a - image.color.a <= 0.001f)
                {
                    image.color = opaque;
                    allowTransition = false;
                }
            }
            else if (mode == FaderMode.FADE_TRANSPARENT)
            {
                image.color = Color.Lerp(image.color, transparent, transitionSpeed * Time.unscaledDeltaTime);

                if (image.color.a <= 0.001f)
                {
                    image.color = transparent;
                    allowTransition = false;
                }
            }
            else if (mode == FaderMode.PING_PONG)
            {
                if (!pingPongBool)
                {
                    image.color = Color.Lerp(image.color, opaque, transitionSpeed * Time.unscaledDeltaTime);

                    if (opaque.a - image.color.a <= 0.001f)
                    {
                        image.color = opaque;
                        pingPongBool = true;
                    }
                }
                else
                {
                    image.color = Color.Lerp(image.color, transparent, transitionSpeed * Time.unscaledDeltaTime);

                    if (image.color.a <= 0.001f)
                    {
                        image.color = transparent;
                        allowTransition = false;
                        pingPongBool = false;
                    }
                }
            }
        }
    }

    public void StartPingPong()
    {
        mode = FaderMode.PING_PONG;
        allowTransition = true;
        pingPongBool = false;
    }
}
