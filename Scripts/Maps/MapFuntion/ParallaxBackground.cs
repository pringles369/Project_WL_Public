using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    public float parallaxEffectX = 0.5f; // X 방향 패럴랙스 효과
    public float parallaxEffectY = 0.5f; // Y 방향 패럴랙스 효과
    public float pixelsPerUnit = 100f;   // 픽셀 퍼 유닛, 게임 해상도에 맞게 설정
    private float length, height, startPosX, startPosY;
    public GameObject cam;

    void Start()
    {
        // 초기 위치를 저장하고, 배경의 크기를 계산
        startPosX = transform.position.x;
        startPosY = transform.position.y;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
        height = GetComponent<SpriteRenderer>().bounds.size.y;
    }

    void Update()
    {
        // 카메라의 이동에 따른 배경의 새로운 위치 계산
        float distX = (cam.transform.position.x - startPosX) * parallaxEffectX;
        float distY = (cam.transform.position.y - startPosY) * parallaxEffectY;

        // 새로운 위치를 계산하여 픽셀 퍼펙트하게 조정
        Vector3 newPosition = new Vector3(startPosX + distX, startPosY + distY, transform.position.z);
        transform.position = PixelPerfectClamp(newPosition, pixelsPerUnit);

        // 수평 반복 효과
        if (cam.transform.position.x > startPosX + (length / 2)) startPosX += length;
        else if (cam.transform.position.x < startPosX - (length / 2)) startPosX -= length;

        // 수직 반복 효과 (필요에 따라)
        if (cam.transform.position.y > startPosY + (height / 2)) startPosY += height;
        else if (cam.transform.position.y < startPosY - (height / 2)) startPosY -= height;
    }

    // 위치를 픽셀 퍼펙트하게 조정하는 메서드
    private Vector3 PixelPerfectClamp(Vector3 locationVector, float pixelsPerUnit)
    {
        Vector3 vectorInPixels = new Vector3(
            Mathf.RoundToInt(locationVector.x * pixelsPerUnit),
            Mathf.RoundToInt(locationVector.y * pixelsPerUnit),
            Mathf.RoundToInt(locationVector.z * pixelsPerUnit)
        );
        return vectorInPixels / pixelsPerUnit;
    }
}