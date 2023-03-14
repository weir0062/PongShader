
Shader "Custom/shader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _NewColor ("NewColor", Color) = (0.69, 0.69, 0.69, 0.69)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness("Smoothness", Range(0,1)) = 0.5
        _Metallic("Metallic", Range(0,1)) = 0.0
        _Position("BallPosition", Vector) = (0,0,0)
        _DotRadius("DotRadius", Range(0.69,6.9)) = 1.69
        _RectCentre("RectCentre", Vector) = (0,0,0)
        _RectHeight("RectHeight", Range(0.69, 6.9)) = 3.69
        _RectWidth("RectWidth", Range(0.69, 6.9)) = 2.69

    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
            float3 worldPos;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        fixed4 _NewColor;
        float3 _Position;
        float _DotRadius;

        float3 _RectCentre;
        float _RectHeight;
        float _RectWidth;
        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;

            float3 p = IN.worldPos;
            float3 delta = p - _Position;
            float dist = delta.x * delta.x + delta.y * delta.y;
             if (dist < _DotRadius* _DotRadius)
             {
                 o.Albedo = _NewColor;
             }



             if (p.x < _RectCentre.x + _RectWidth / 2 &&
              p.x > _RectCentre.x - _RectWidth / 2 &&
              p.y < _RectCentre.y + _RectHeight / 2 &&
              p.y > _RectCentre.y - _RectHeight / 2 )
             {
                 o.Albedo = _NewColor;
             }
        

        }
        ENDCG
    }
    FallBack "Diffuse"
}
