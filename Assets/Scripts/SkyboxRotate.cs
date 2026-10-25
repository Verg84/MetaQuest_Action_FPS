using UnityEngine;

public class SkyboxRotate : MonoBehaviour
{
    [SerializeField] private float degreesPerSecond = 0.1f;

    private Material originalSkybox;
    private Material animatedSkybox;
    private float rotation;

    private void OnEnable()
    {
        originalSkybox = RenderSettings.skybox;

        if (originalSkybox == null)
            return;

        animatedSkybox = new Material(originalSkybox);
        rotation = animatedSkybox.GetFloat("_Rotation");
        RenderSettings.skybox = animatedSkybox;
    }

    private void Update()
    {
        if (animatedSkybox == null)
            return;

        rotation = Mathf.Repeat(
            rotation + degreesPerSecond * Time.deltaTime,
            360f
        );

        animatedSkybox.SetFloat("_Rotation", rotation);
    }

    private void OnDisable()
    {
        if (animatedSkybox == null)
            return;

        if (RenderSettings.skybox == animatedSkybox)
            RenderSettings.skybox = originalSkybox;

        Destroy(animatedSkybox);
    }
}