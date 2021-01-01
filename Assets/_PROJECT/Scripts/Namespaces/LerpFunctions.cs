using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

namespace LerpFunctions
{
    [Serializable]
    public class OffsetGroup
    {
        public Vector2 min;
        public Vector2 max;

        public OffsetGroup()
        {
            min = Vector2.zero;
            max = Vector2.zero;
        }

        public OffsetGroup(Vector2 n, Vector2 x)
        {
            min = n;
            max = x;
        }

        public OffsetGroup(RectTransform rect)
        {
            min = rect.offsetMin;
            max = rect.offsetMax;
        }

        public OffsetGroup(Rect rect)
        {
            min = new Vector2(rect.x, rect.y - rect.height);
            max = new Vector2(rect.x + rect.width, rect.y);
        }

        public OffsetGroup(OffsetGroup copy)
        {
            min = copy.min;
            max = copy.max;
        }

        public void AddWidth(float width)
        {
            min.x += width;
            max.x += width;
        }

        public void AddHeight(float height)
        {
            min.y += height;
            min.y += height;
        }

        public static void Copy(RectTransform target, OffsetGroup other)
        {
            target.offsetMin = other.min;
            target.offsetMax = other.max;
        }
    }

    [Serializable]
    public class FlexibleRect
    {
        public RectTransform rectTransform;
        public Vector2 originalPosition;
        public Vector2 endPosition;

        bool allowTransition = false;

        public FlexibleRect(RectTransform rectTr)
        {
            rectTransform = rectTr;
            originalPosition = center;
            endPosition = Vector2.zero;
        }

        public void Step(float speed, float precision = 0.1f)
        {
            if (allowTransition)
            {
                allowTransition = !Lerp(endPosition, speed, precision);
            }
        }

        public void StartLerp(Vector2 endPos)
        {
            allowTransition = true;
            endPosition = endPos;
        }

        public void EndLerp()
        {
            allowTransition = false;
        }

        public bool Lerp(Vector2 targetPosition, float speed, float precision = 0.1f)
        {
            Vector2 destination = Vector2.Lerp(center, targetPosition, speed);

            if ((targetPosition - destination).sqrMagnitude <= precision * precision)
            {
                MoveTo(targetPosition);
                return true;
            }

            MoveTo(destination);
            return false;
        }

        public void MoveTo(Vector2 targetPosition)
        {
            Vector2 diff = targetPosition - center;
            rectTransform.offsetMax += diff;
            rectTransform.offsetMin += diff;
        }

        public Vector2 GetBodyOffset(Vector2 direction)
        {
            return new Vector2(originalPosition.x + (direction.x * width), originalPosition.y + (direction.y * height));
        }

        public Vector2 GetBodyOffset(Vector2 direction, float degreeOfSelf)
        {
            return new Vector2(originalPosition.x + (direction.normalized.x * degreeOfSelf * halfWidth), originalPosition.y + (direction.normalized.y * degreeOfSelf * halfHeight));
        }

        public Vector2 ofMin => rectTransform.offsetMin;
        public Vector2 ofMax => rectTransform.offsetMax;
        public Vector2 center => (ofMin + ofMax) * 0.5f;
        public Vector2 centerPivoted => (ofMin + ofMax) * 0.5f * rectTransform.pivot;
        public float width => rectTransform.rect.width;
        public float height => rectTransform.rect.height;
        public float halfWidth => width * 0.5f;
        public float halfHeight => height * 0.5f;
    }

    [Serializable]
    public class CanvasGroupFader
    {
        enum CanvasState
        {
            FADE_OUT,
            FADE_IN
        }

        public CanvasGroup canvas;
        public bool affectsTouch;
        public float precision;

        CanvasState state;
        bool allowFade;

        public CanvasGroupFader(CanvasGroup canvasGroup, bool startsFadeIn, bool canAffectTouch, float alphaPrecision = 0.001f)
        {
            canvas = canvasGroup;
            affectsTouch = canAffectTouch;
            if (startsFadeIn) SetStateFadeIn();
            else SetStateFadeOut();

            allowFade = false;
            precision = alphaPrecision;
        }

        public void Step(float speed)
        {
            if (allowFade)
            {
                if (state == CanvasState.FADE_IN)
                {
                    canvas.alpha = Mathf.Lerp(canvas.alpha, 1f, speed);

                    if (1f - canvas.alpha <= precision)
                    {
                        allowFade = false;
                        canvas.alpha = 1f;

                        if (affectsTouch)
                        {
                            canvas.blocksRaycasts = true;
                            canvas.interactable = true;
                        }
                    }
                }
                else
                {
                    canvas.alpha = Mathf.Lerp(canvas.alpha, 0f, speed);

                    if (canvas.alpha <= precision)
                    {
                        allowFade = false;
                        canvas.alpha = 0f;

                        if (affectsTouch)
                        {
                            canvas.blocksRaycasts = false;
                            canvas.interactable = false;
                        }
                    }
                }
            }
        }

        public void SetAlpha(float alpha)
        {
            canvas.alpha = alpha;
        }

        public void StartFadeIn()
        {
            SetStateFadeIn();
            TriggerFade();
        }

        public void StartFadeOut()
        {
            SetStateFadeOut();
            TriggerFade();
        }

        public void TriggerFade() => allowFade = true;
        public void SetStateFadeIn() => state = CanvasState.FADE_IN;
        public void SetStateFadeOut() => state = CanvasState.FADE_OUT;
        public bool isFading => allowFade;
    }

    public class Lerp : MonoBehaviour
    {
        // rects
        public static bool Rect(OffsetGroup targetOffset, RectTransform movingObject, float lerpSpeed = 1f)
        {
            Vector2 botLeft = Vector2.Lerp(movingObject.offsetMin, targetOffset.min, lerpSpeed * Time.deltaTime);
            Vector2 topRight = Vector2.Lerp(movingObject.offsetMax, targetOffset.max, lerpSpeed * Time.deltaTime);

            if (NegligibleDistance(movingObject.offsetMin, targetOffset.min))
            {
                movingObject.offsetMin = targetOffset.min;
                movingObject.offsetMax = targetOffset.max;
                return true;
            }

            movingObject.offsetMin = botLeft;
            movingObject.offsetMax = topRight;
            return false;
        }
        public static bool Rect(RectTransform targetRect, RectTransform movingObject, float lerpSpeed = 1f)
        {
            Vector2 botLeft = Vector2.Lerp(movingObject.offsetMin, targetRect.offsetMin, lerpSpeed * Time.deltaTime);
            Vector2 topRight = Vector2.Lerp(movingObject.offsetMax, targetRect.offsetMax, lerpSpeed * Time.deltaTime);

            if (NegligibleDistance(movingObject.offsetMin, targetRect.offsetMin))
            {
                movingObject.offsetMin = targetRect.offsetMin;
                movingObject.offsetMax = targetRect.offsetMax;
                return true;
            }

            movingObject.offsetMin = botLeft;
            movingObject.offsetMax = topRight;
            return false;
        }

        // float
        public static bool Float(float a, float b, float lerpSpeed = 1f)
        {
            if (b < a)
                a = Mathf.Lerp(b, a, lerpSpeed * Time.deltaTime);
            else
                a = Mathf.Lerp(a, b, lerpSpeed * Time.deltaTime);

            if (NegligibleDistance(a, b))
            {
                a = b;
                return true;
            }

            return false;
        }

        // anchored position
        public static bool APosition(RectTransform obj, Vector2 targetPos, float lerpSpeed = 1f)
        {
            obj.anchoredPosition = Vector2.Lerp(obj.anchoredPosition, targetPos, lerpSpeed * Time.deltaTime);
            obj.anchoredPosition = new Vector2((float)Math.Round(obj.anchoredPosition.x, 1), (float)Math.Round(obj.anchoredPosition.y, 1));

            if (NegligibleDistance(obj.anchoredPosition, targetPos))
            {
                obj.anchoredPosition = targetPos;
                return true;
            }
            
            return false;
        }

        // position
        public static bool Position(RectTransform obj, Vector2 targetPos, float lerpSpeed = 1f)
        {
            obj.position = Vector2.Lerp(obj.position, targetPos, lerpSpeed * Time.deltaTime);
            obj.position = new Vector2((float)Math.Round(obj.position.x, 1), (float)Math.Round(obj.position.y, 1));

            if (NegligibleDistance(obj.position, targetPos))
            {
                obj.position = targetPos;
                return true;
            }

            return false;
        }

        // offset position
        public static bool OFPosition(RectTransform target, OffsetGroup destination, float lerpSpeed = 1f)
        {
            target.offsetMin = Vector2.Lerp(target.offsetMin, destination.min, lerpSpeed * Time.deltaTime);
            target.offsetMax = Vector2.Lerp(target.offsetMax, destination.max, lerpSpeed * Time.deltaTime);

            if (NegligibleDistance(target.offsetMin, destination.min))
            {
                target.offsetMin = destination.min;
                target.offsetMax = destination.max;
                return true;
            }

            return false;
        }

        // vector
        public static bool Vector(Vector2 target, Vector2 destination, float lerpSpeed = 1f)
        {
            target = Vector2.Lerp(target, destination, lerpSpeed * Time.deltaTime);

            if (NegligibleDistance(target, destination))
            {
                target = destination;
                return true;
            }

            return false;
        }

        // size delta
        public static bool SizeDelta(RectTransform target, Vector2 targetSizeDelta, float lerpSpeed = 1f)
        {
            target.sizeDelta = Vector2.Lerp(target.sizeDelta, targetSizeDelta, lerpSpeed * Time.deltaTime);

            if (NegligibleDistance(target.sizeDelta, targetSizeDelta))
            {
                target.sizeDelta = targetSizeDelta;
                return true;
            }

            return false;
        }

        // offscreens
        public static bool OffScreenBelow(RectTransform target, Vector2 offsetMin, float lerpSpeed = 1f)
        {
            Vector2 minDest = new Vector2(target.offsetMin.x, -Screen.height);
            Vector2 maxDest = new Vector2(target.offsetMax.x, 0);

            target.offsetMin = Vector2.Lerp(target.offsetMin, minDest, lerpSpeed * Time.deltaTime);
            target.offsetMax = Vector2.Lerp(target.offsetMax, maxDest, lerpSpeed * Time.deltaTime);

            if (NegligibleDistance(target.offsetMin, minDest))
            {
                target.offsetMin = minDest;
                target.offsetMax = maxDest;
                return true;
            }

            return false;
        }
        public static bool OffScreenBelow(RectTransform target, OffsetGroup offsetGrp, float lerpSpeed = 1f)
        {
            Vector2 minDest = new Vector2(target.offsetMin.x, Screen.height);
            Vector2 maxDest = new Vector2(target.offsetMax.x, 0);

            target.offsetMin = Vector2.Lerp(target.offsetMin, minDest, lerpSpeed * Time.deltaTime);
            target.offsetMax = Vector2.Lerp(target.offsetMax, maxDest, lerpSpeed * Time.deltaTime);

            if (NegligibleDistance(target.offsetMin, minDest))
            {
                target.offsetMin = minDest;
                target.offsetMax = maxDest;
                return true;
            }

            return false;
        }

        // direct movements
        public static void ForceStay(GameObject obj, Vector3 forcedPos)
        {
            obj.transform.position = forcedPos;
        }
        public static void Warp(RectTransform target, OffsetGroup destination)
        {
            target.offsetMax = destination.max;
            target.offsetMin = destination.min;
        }
        public static void WarpOffScreenBelow(RectTransform target, Vector2 offsetMin)
        {
            target.offsetMin = new Vector2(target.offsetMin.x, -Screen.height);
            target.offsetMax = new Vector2(target.offsetMax.x, 0);
        }
        public static void WarpOffScreenBelow(RectTransform target, OffsetGroup offsetGrp)
        {
            target.offsetMin = new Vector2(target.offsetMin.x, -Screen.height);
            target.offsetMax = new Vector2(target.offsetMax.x, 0);
        }
        public static void Follow(RectTransform followTarget, RectTransform follower, bool stopChildren = false)
        {
            follower.offsetMax = followTarget.offsetMax;
            follower.offsetMin = followTarget.offsetMin;

            if (stopChildren)
            {
                for (int i = 0; i < follower.childCount; i++)
                {

                }
            }    
        }

        // queries
        public static bool OnPosition(Vector2 target1, Vector2 target2) => target1 == target2;
        public static bool OnAPosition(RectTransform target1, RectTransform target2) => target1.anchoredPosition == target2.anchoredPosition;
        public static bool OnAPosition(RectTransform target1, Vector2 target2) => target1.anchoredPosition == target2;
        public static bool OnOFPosition(RectTransform target1, RectTransform target2) => target1.offsetMax == target2.offsetMax && target1.offsetMin == target2.offsetMin;
        //public static bool NegligibleDistance(Vector2 a, Vector2 b, float condition = 0.1f) => Vector2.Distance(a, b) < condition;
        public static bool NegligibleDistance(Vector3 a, Vector3 b, float condition = 0.1f) => Vector3.Distance(a, b) < condition;
        public static bool NegligibleDistance(float a, float b, float condition = 0.1f) => Mathf.Abs(a - b) < condition;

        #region QUATENIONS
        public static bool Rotation(RectTransform target, Quaternion rotation, float lerpSpeed = 1f)
        {
            target.rotation = Quaternion.Lerp(target.rotation, rotation, lerpSpeed * Time.deltaTime);

            if (NegligibleDistance(target.rotation.x, rotation.x, 0.01f))
            {
                target.rotation = rotation;
                return true;
            }

            return false;
        }
        #endregion

        #region COLORS
        // textmeshpro
        public static bool AlphaTMP(TextMeshProUGUI target, float alpha, float lerpSpeed = 1f)
        {
            float a = Mathf.Lerp(target.color.a, alpha, lerpSpeed * Time.deltaTime);
            target.color = new Color(target.color.r, target.color.g, target.color.b, a);

            if (NegligibleDistance(target.color.a, alpha, 0.01f))
            {
                target.color = new Color(target.color.r, target.color.g, target.color.b, alpha);
                return true;
            }

            return false;
        }

        // image
        public static bool AlphaImage(Image img, float alpha, float lerpSpeed = 1f)
        {
            Color target = new Color(img.color.r, img.color.g, img.color.b, alpha);
            img.color = Color.Lerp(img.color, target, lerpSpeed * Time.deltaTime); 

            if (NegligibleDistance(img.color.a, target.a))
            {
                img.color = target;
                return true;
            }

            return false;
        }

        // canvas group
        public static bool AlphaCanvasGroup(CanvasGroup group, float alpha, float lerpSpeed = 1f)
        {
            group.alpha = Mathf.Lerp(group.alpha, alpha, lerpSpeed * Time.deltaTime);

            if (NegligibleDistance(group.alpha, alpha, 0.01f))
            {
                group.alpha = alpha;
                return true;
            }

            return false;
        }

        // alpha jumps
        public static void AlphaJump(CanvasGroup group, float alpha) => group.alpha = alpha;
        public static void AlphaJump(Image image, float alpha) => image.color = new Color (image.color.r, image.color.g, image.color.b, alpha);
        public static void AlphaJump(TextMeshProUGUI tmp, float alpha) => tmp.color = new Color(tmp.color.r, tmp.color.g, tmp.color.b, alpha);
        #endregion

        #region SCALING
        public static bool Scale(RectTransform target, Vector3 scaleSize, float lerpSpeed = 1f, float negCondition = 0.1f)
        {
            if (target.localScale.x > scaleSize.x)
                target.localScale = Vector3.Lerp(target.localScale, scaleSize, lerpSpeed * Time.deltaTime);
            else
                target.localScale = Vector3.Lerp(scaleSize, target.localScale, lerpSpeed * Time.deltaTime);

            if (NegligibleDistance(target.localScale, scaleSize, negCondition))
            {
                target.localScale = scaleSize;
                return true;
            }

            return false;
        }
        public static bool Scale(RectTransform target, float scale, float lerpSpeed = 1f, float negCondition = 0.1f)
        {
            Vector3 scaleSize = new Vector3(scale, scale, scale);
            /*if (target.localScale.x > scale)
                target.localScale = Vector3.Lerp(target.localScale, scaleSize, lerpSpeed * Time.deltaTime);
            else
                target.localScale = Vector3.Lerp(scaleSize, target.localScale, lerpSpeed * Time.deltaTime);*/
            target.localScale = Vector3.Lerp(target.localScale, scaleSize, lerpSpeed * Time.deltaTime);

            if (NegligibleDistance(target.localScale, scaleSize, negCondition))
            {
                target.localScale = scaleSize;
                return true;
            }

            return false;
        }
        public static void SizeSet(RectTransform target, float scale)
        {
            target.localScale = new Vector3(scale, scale, scale);
        }

        #endregion
    }

    public class Get : MonoBehaviour
    {
        public static RectTransform RectTr(Component g) => g.GetComponent<RectTransform>();
        public static RectTransform RectTransform(Component g) => g.GetComponent<RectTransform>();
        public static CanvasGroup CG(Component g) => g.GetComponent<CanvasGroup>();
        public static CanvasGroup CanvasGroup(Component g) => g.GetComponent<CanvasGroup>();
        public static Transform Tr(Component g) => g.GetComponent<Transform>();
        public static Transform Transform(Component g) => g.GetComponent<Transform>();
    }
}

