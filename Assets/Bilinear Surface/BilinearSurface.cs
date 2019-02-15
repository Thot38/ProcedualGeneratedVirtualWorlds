using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

/// <summary>
/// Das BilinearSurface
/// 
/// Ein BilinearSurface ist eine aus 4 Kontrollpunkten interpolierte
/// Oberfläche (surface). Durch die Anordnung der 4 Kontrollpunkte im Raum,
/// können über Linearinterpolationen "gekrümmte" Flächen erzeugt werden.
/// 
/// Die Eckpunkte sind P00, P01, P10 und P11:
/// 
///     P01-----------P11
///      |             |
///      |             |
///      |             |
///      |             |
///      |             |
///     P00-----------P10
/// 
/// Wir können das Surface mit zwei Parametern  u  und  w  parametrisieren.
/// 
///  1   P01-----------P11
///  |    |             |
///  |    |             |
/// w|    |             |
///  |    |             |
///  |    |             |
///  0   P00-----------P10
///
///       0-------------1
///              u       
/// 
/// Möchten wir einen Punkt auf dem Surface berechnen, können wir dies
/// durch Linearinterpolationen erreichen.
/// 
///  1   P01-----------P11
///  |    |             |
///  |----|---------o   |    Der Punkt in der Skizze hat die Parameter:
/// w|    |         |   |    u = 0.8f
///  |    |         |   |    w = 0.8f
///  |    |         |   |
///  0   P00-----------P10
///                 |
///       0-------------1       
///              u
/// 
/// Wir können nun eine Linearinterpolation für das Segment P00 --> P10 mit
/// dem Parameter u machen und erhalten einen Punkt P_1 auf dem Segement.
/// Das gleiche machen wir für das Segment P01 --> P11 mit dem Parameter u
/// und erhalten den Punkt P_2.
/// 
///  1   P01--------o--P11
///  |    |             |
///  |    |         o   |    Der Punkt in der Skizze hat die Parameter:
/// w|    |             |    u = 0.8f
///  |    |             |    w = 0.8f
///  |    |             |
///  0   P00--------o--P10
///                 
///       0-------------1       
///              u
/// 
/// Jetzt können wir auf dem Segment von P_1 --> P_2  bei Parameter  w  durch
/// eine Linearinterpolation einen Punkt berechnen. 
/// Dieser ist unser gewünschter Punkt bei den Parametern  u und w.
///  
///                P_2
///  1   P01--------o--P11
///  |    |         |   |
///  |    |         o   |    Der Punkt in der Skizze hat die Parameter:
/// w|    |         |   |    u = 0.8f
///  |    |         |   |    w = 0.8f
///  |    |         |   |
///  0   P00--------o--P10
///                P_1
///       0-------------1       
///              u
/// 
/// Wenn wir nach diesem Prinzip ein Raster über das Surface legen, 
/// erhalten wir Punkte, die wir mit Dreiecken zu einer echten Fläche
/// zusammensetzen können.
/// 
///  1    o-o-o-o-o-o-o-o
///  |    o-o-o-o-o-o-o-o
///  |    o-o-o-o-o-o-o-o
/// w|    o-o-o-o-o-o-o-o
///  |    o-o-o-o-o-o-o-o
///  |    o-o-o-o-o-o-o-o
///  0    o-o-o-o-o-o-o-o
///
///       0-------------1
/// 
/// </summary>
public class BilinearSurface : MonoBehaviour
{
    // Die 4 Kontrollpunkte (Ecken) des BilinearSurface
    public Vector3 P00 = new Vector3(0f, 0f, 0f);
    public Vector3 P01 = new Vector3(0f, 0f, 10f);
    public Vector3 P10 = new Vector3(10f, 0f, 0f);
    public Vector3 P11 = new Vector3(10f, 0f, 10f);

    int textureDimensions = 512;
    public Texture2D mainTexture;
    public Texture2D heightMapTexture;
    public Texture2D noiseMapTexture;

    // Die Parameter u und w die zur Demonstration
    // dazu verwendet werden einen Punkt auf der Fläche
    // zu verechnen
    // Die Annotation Range gibt den Wertebereich an, 
    // der im Inspektor eingestellt werden kann.
    [Range(0f, 1f)]
    public float u = 1f / 3f;
    [Range(0f, 1f)]
    public float w = 1f / 3f;

    public float amplitude = 2;
    public float roughidity = 5;

    // Die Anzahl der Segmente in u- und w-Richtung,
    // die dazu verwendet werden das Raster von Punkten
    // zu Erzeugen.
    // Die Annotation Range gibt den Wertebereich an, 
    // der im Inspektor eingestellt werden kann.
    [Range(1, 100)]
    public int numberOfSegmentsU = 90;
    [Range(1, 100)]
    public int numberOfSegmentsW = 45;

    // Methode zum Zeichnen der Gizmos des BilinearSurface:
    // - Es sollen die Kontrollpunkte als Spheres gezeichnet werden
    // - Die "Umrandung" des Surfaces soll mit Lines gezeichnet werden.
    // - Es soll ein Punkt mit den Parametern u und w berechnet und 
    //   als Sphere gezeichnet werden.
    // - Es soll ein Raster an Punkten gezeichnet werden
    //   Hierzu soll in u-Richtung und in w-Richtung die Anzahl der
    //   Punkte (Anzahl Segmente +1) einstellbar sein.
    public void OnDrawGizmos()
    {
        // Zeichne die "Umrandung" mit grauen Linien
        Gizmos.color = Color.grey;
        Gizmos.DrawLine(P00, P01);
        Gizmos.DrawLine(P01, P11);
        Gizmos.DrawLine(P11, P10);
        Gizmos.DrawLine(P10, P00);

        // Zeichne die Kontrollpunkte in grün
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(P00, 0.1f);
        Gizmos.DrawSphere(P01, 0.1f);
        Gizmos.DrawSphere(P10, 0.1f);
        Gizmos.DrawSphere(P11, 0.1f);

        //if (vertices != null)
        //{
        //    foreach (var v in vertices)
        //    {
        //        Gizmos.DrawSphere(v, 0.1f);
        //    }
        //}

        // Zeichne den Punkt bei Parameter u und w auf dem Surface
        // (u und w können im Inspektor angepasst werden)
        Gizmos.DrawSphere(
            PointOnBilinearSurface(
                P00, P01, P10, P11, u, w), 0.1f);
    }

    /// <summary>
    /// Methode um einen Punkt auf einem BilinearSurface zu berechnen.
    /// Das Surface wird durch die Punkte P00, P01, P10 und P11 definiert.
    /// Die Berechnung des Punktes bei den Parametern u und w erfolgt durch
    /// Linearinterpolationen.
    /// </summary>
    /// <param name="p00"></param>
    /// <param name="p01"></param>
    /// <param name="p10"></param>
    /// <param name="p11"></param>
    /// <param name="u"></param>
    /// <param name="w"></param>
    /// <returns></returns>
    private Vector3 PointOnBilinearSurface(
        Vector3 p00, Vector3 p01, Vector3 p10, Vector3 p11,
        float u, float w)
    {
        // Variante 1:
        // Berechnung durch direkte Linearinterpolation
        // Mit Parameter u wird für P00-->P01 und P10-->P11 jeweils 
        // ein Punkt berechnet.
        Vector3 p_1 = PointOnLine(p00, p10, u);
        Vector3 p_2 = PointOnLine(p01, p11, u);

        // Die berechneten Punkte definieren ein neues Segment in w-Richtung
        // Auf diesem Segment P_1-->P_2 berechnen wir mit Parameter w den
        // Punkt auf der Fläche.
        Vector3 point = PointOnLine(p_1, p_2, w);

        //// ##############################################
        //// Variante 2:
        //// Zusammengefasste Form der LinearInterpolationen
        //Vector3 point = p00 * (1 - u) * (1 - w) +
        //    p01 * (1 - u) * w +
        //    p10 * u * (1 - w) +
        //    p11 * u * w;


        //// ##############################################
        //// Variante 3:
        //// Optimierte Form der Berechnung, durch Vorberechnung mehrfach
        //// auftretender Werte
        //float oneMinusU = 1 - u;
        //float oneMinusW = 1 - w;

        //Vector3 point = p00 * oneMinusU * oneMinusW +
        //    p01 * oneMinusU * w +
        //    p10 * u * oneMinusW +
        //    p11 * u * w;

        return point;
    }

    /// <summary>
    /// Berechnung eines Punktes auf einem Dreieck.
    /// Das Dreieck wird durch die Punkte P0, P1 und P2 definiert.
    /// Die Parameter u, v und w bestimmen wie stark die Punkte des
    /// Dreiecks Einfluss auf die Koordinaten des Punktes haben.
    /// Es wird in diesem Zusammenhang von der gewichteten Kombination
    /// der Eckpunkte gesprochen, da die Summe der Ortsvektoren der
    /// Eckpunkte mit den Faktoren u, v und w skaliert werden.
    /// Ergibt die Summe der Faktoren 1, so liegt der Punkt auf der
    /// Dreiecksfläche.
    /// 
    /// Die Koordinaten u, v und w werden als Baryzentrische Koordinaten
    /// bezeichnet. (siehe: David Salomon, "Curves and Surfaces for
    /// Computer Graphics", Springer Verlag)
    ///
    /// Kurz:
    /// Diese Methode Berechnet eine gewichtete Summe der drei Eckpunkte eines
    /// Triangles. Die gewichte sind durch die Parameter  u, v und w  gegeben.
    /// Ist die Summe  v + u + w = 1  gegeben, so befindet sich der berechnete
    /// Punkt auf der Dreiecksfläche.
    /// 
    /// </summary>
    /// <param name="p0">Punkt P0 des Dreiecks</param>
    /// <param name="p1">Punkt P1 des Dreiecks</param>
    /// <param name="p2">Punkt P2 des Dreiecks</param>
    /// <param name="u">Der Parameter u</param>
    /// <param name="v">Der Parameter v</param>
    /// <param name="w">Der Parameter w</param>
    /// <returns></returns>
    private Vector3 PointOnTriangle(
        Vector3 p0, Vector3 p1, Vector3 p2,
        float u, float v, float w)
    {
        return u * p0 + v * p1 + w * p2;
    }

    /// <summary>
    /// Methode um einen Punkt auf einer Linie zu berechnen.
    /// Die Linie bzw. der Strahl wird durch die Punkte P0 und P1 definiert.
    /// P0 kann als Start und P1 als Ende angesehen werden.
    /// Die Richtung bzw. der Richtungsvektor des Strahls verläuft von
    /// P0 in Richtung P1
    /// Der Parameter t bestimmt den zu berechnenden Punkt auf dem Strahl.
    /// 
    /// Wird der Parameter zwischen 0 und 1 gewählt, liegt der berechnete
    /// Punkt auf dem durch P0 und P1 definierten Segment.
    /// 
    /// Wird der Parameter < 0  oder > 1 gewählt, liegt der Punkt auf dem
    /// durch P0 und P1 definierten Strahl. Ist t > 1 liegt der Punkt 
    /// </summary>
    /// <param name="p0"></param>
    /// <param name="p1"></param>
    /// <param name="t"></param>
    /// <returns></returns>
    private Vector3 PointOnLine(Vector3 p0, Vector3 p1, float t)
    {
        return (1 - t) * p0 + t * p1;
    }

    private void Update()
    {
        if (mesh == null)
        {
            CreateMesh();
        }
    }

    List<Vector3> vertices;
    List<Vector3> sphereVertices;
    List<int> triangles;
    List<Vector2> uvs;
    MeshFilter meshFilter;
    MeshRenderer meshRenderer;
    Mesh mesh;
    Material material;

    public void CreateMesh()
    {
        #region Init stuff
        meshFilter = GetComponent<MeshFilter>();
        if (meshFilter == null)
            meshFilter = gameObject.AddComponent<MeshFilter>();

        meshFilter.sharedMesh = mesh = new Mesh();

        meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer == null)
            meshRenderer = gameObject.AddComponent<MeshRenderer>();

        if (material == null)
        {
            material = new Material(Shader.Find("Standard"));
        }

        //if (heightMapTexture == null)
        //{
        //    Resources.Load<Texture2D>("heightmap");
        //}
        //if (mainTexture == null)
        //{
        //    Resources.Load<Texture2D>("maintexture");
        //}

        material.mainTexture = mainTexture;
        meshRenderer.sharedMaterial = material;

        #endregion

        var numberOfPointsU = numberOfSegmentsU + 1;
        var numberOfPointsW = numberOfSegmentsW + 1;

        heightMapTexture = CreateNoiseHeightMap();
        mainTexture = CreateMainTextureFromNoise();

        vertices = new List<Vector3>();
        sphereVertices = new List<Vector3>();
        uvs = new List<Vector2>();

        float stepU = 1f / (float)numberOfSegmentsU;
        float stepW = 1f / (float)numberOfSegmentsW;

        float widthStepU = 360f / (float)numberOfSegmentsU;
        float heightStepW = 180f / (float)numberOfSegmentsW; 

        for (int u = 0; u < numberOfPointsU; u++)
        {
            for (int w = 0; w < numberOfPointsW; w++)
            {
                float _u = u * stepU;
                float _w = w * stepW;

                var heightColor = heightMapTexture.GetPixelBilinear(_u, _w);
                var height = heightColor.grayscale * amplitude;

                var point = PointOnBilinearSurface(P00, P01, P10, P11, _u, _w);
                point.y = height;

                vertices.Add(point);

                var lon = u * widthStepU;
                var lat = w * heightStepW;
                var radius = 10f;

                var spherePoint = PointOnSphere(lon, lat, radius + height);
                sphereVertices.Add(spherePoint);

                uvs.Add(new Vector2(_u, _w));
            }
        }
        mesh.SetVertices(sphereVertices);
        mesh.uv = uvs.ToArray();
        triangles = new List<int>();

        for(int u = 0; u < numberOfSegmentsU; u++)
        {
            for (int w = 0; w < numberOfSegmentsW; w++)
            {
                var p0 = u * numberOfPointsW + w;
                var p1 = u * numberOfPointsW + (w + 1);
                var p2 = (u + 1) * numberOfPointsW + w;
                var p3 = (u + 1) * numberOfPointsW + (w + 1);

                triangles.Add(p0);
                triangles.Add(p1);
                triangles.Add(p2);

                mesh.triangles = triangles.ToArray();

                triangles.Add(p2);
                triangles.Add(p1);
                triangles.Add(p3);

                mesh.triangles = triangles.ToArray();
            }
        }
        mesh.RecalculateNormals();
    }

    public Texture2D grassTexture;
    public Texture2D rockTexture;
    public Texture2D waterTexture;

    public float waterlevel = 0.3f;
    public float grasslevel = 0.5f;
    
    private Texture2D CreateMainTextureFromNoise()
    {
        mainTexture = new Texture2D(textureDimensions, textureDimensions);
        mainTexture.name = "Günter";

        if (grassTexture == null || rockTexture == null || waterTexture == null)
        {
            Debug.LogWarning("Du bist ein Pleb");
            return mainTexture;
        }

        for (int i = 0; i < textureDimensions; i++)
        {
            for (int j = 0; j < textureDimensions; j++)
            {
                var normalSample = heightMapTexture.GetPixel(i, j);

                if (normalSample.grayscale < waterlevel)
                {
                    heightMapTexture.SetPixel(i, j, new Color(waterlevel, waterlevel, waterlevel));
                    mainTexture.SetPixel(i, j, waterTexture.GetPixel(i % waterTexture.width, j % waterTexture.height));
                }
                else
                {
                    var lerpParameter = 1f - (normalSample.grayscale - grasslevel) / (1f - grasslevel);

                    var colorBlendGrasshill = Color.Lerp(grassTexture.GetPixel(i % grassTexture.width, j % grassTexture.height), rockTexture.GetPixel(i % rockTexture.width, j % rockTexture.height), lerpParameter);
                    mainTexture.SetPixel(i, j, colorBlendGrasshill);
                }
                //}
                //else if (normalSample.grayscale < grasslevel)
                //{
                //    mainTexture.SetPixel(i, j, grassTexture.GetPixel(i % grassTexture.width, j % grassTexture.height));
                //}
                //else
                //{
                //    mainTexture.SetPixel(i, j, rockTexture.GetPixel(i % rockTexture.width, j % rockTexture.height));
                //}
            }
        }
        mainTexture.Apply();
        return mainTexture;
    }

    private Texture2D CreateNoiseHeightMap()
    {
        noiseMapTexture = new Texture2D(textureDimensions, textureDimensions);
        noiseMapTexture.name = "NoiseTex";

        for (int i = 0; i < textureDimensions; i++)
        {
            for (int j = 0; j < textureDimensions; j++)
            {
                var grayValue = Mathf.PerlinNoise(i * roughidity / (float) textureDimensions, j * roughidity / (float) textureDimensions);

                noiseMapTexture.SetPixel(i, j, new Color(grayValue, grayValue, grayValue));
            }
        }
        noiseMapTexture.Apply();
        return noiseMapTexture;
    }

    private Vector3 PointOnSphere(float lon, float lat, float radius)
    {
        var lonRad = lon * Mathf.Deg2Rad;
        var latRad = lat * Mathf.Deg2Rad;

        var x = Mathf.Cos(lonRad) * Mathf.Sin(latRad);
        var y = Mathf.Sin(lonRad) * Mathf.Sin(latRad);
        var z = Mathf.Cos(latRad);

        return RotateAroundPivot(new Vector3(x, y, z) * radius, transform.position, new Vector3(90f, 0f, 0f));
    }

    public Vector3 RotateAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
    {
        var direction = point - pivot;
        direction = Quaternion.Euler(angles) * direction;
        point = pivot + direction;
        return point;
    }
}
