Shader "FieldOfView" {
	Properties{
		// Properties of the material
		_MainTex("Base (RGB)", 2D) = "white" {}
		_FOVColor("Field Of View Color", Color) = (1, 1, 1)
		_MainColor("MainColor", Color) = (1, 1, 1)
		_Position("Position",  Vector) = (0,0,0)
		_Direction("Direction",  Vector) = (0,0,0)
	}
		SubShader{
		Tags{ "RenderType" = "Diffuse" }
		// https://docs.unity3d.com/Manual/SL-SurfaceShaders.html
		CGPROGRAM
#pragma surface surf Lambert

	sampler2D _MainTex;
		//https://docs.unity3d.com/Manual/SL-DataTypesAndPrecision.html
		fixed3 _FOVColor; //Precision
		fixed3 _MainColor;
		float3 _Position;
		float3 _Direction;

		// Values that interpolated from vertex data.
		struct Input {
			float2 uv_MainTex;
			float3 worldPos;
		};

		// Barycentric coordinates
		// http://mathworld.wolfram.com/BarycentricCoordinates.html
		bool isPointInTriangle(float2 p1, float2 p2, float2 p3, float2 pointInQuestion)
		{
			float denominator = ((p2.y - p3.y) * (p1.x - p3.x) + (p3.x - p2.x) * (p1.y - p3.y));
			float a = ((p2.y - p3.y) * (pointInQuestion.x - p3.x) + (p3.x - p2.x) * (pointInQuestion.y - p3.y)) / denominator;
			float b = ((p3.y - p1.y) * (pointInQuestion.x - p3.x) + (p1.x - p3.x) * (pointInQuestion.y - p3.y)) / denominator;
			float c = 1 - a - b;

			return 0 <= a && a <= 1 && 0 <= b && b <= 1 && 0 <= c && c <= 1;
		}

		bool isPointInTheCircle(float2 circleCenterPoint, float2 thisPoint, float radius)
		{
			return distance(circleCenterPoint, thisPoint) <= radius;
		}

		void surf(Input IN, inout SurfaceOutput o) {
			half4 c = tex2D(_MainTex, IN.uv_MainTex);

			float offsetAngle = 30.0;
			float offsetAngleInRadians = offsetAngle * (3.14 / 180);
			float distanceVar = 10.0;

			float3 basePoint = _Position.xyz;
			basePoint.y = 0;

			//float3 cameraDir = -1 * UNITY_MATRIX_IT_MV[2].xyz;
			float3 cameraDir = _Direction;
			float viewAngle = atan2(cameraDir.z, cameraDir.x);

			//Find distance of that needed for side if we know our base height
			//https://www.mathsisfun.com/algebra/trig-finding-side-right-triangle.html

			//Cos 
			//We have Adjacent its our distance var
			//cos(0) = Adjacent / Hypotenuse
			//We need to find hypotenuse so

			//10 = 20 / 2
			//cos(radians) = distanceVar/ Hypotenuse
			//2 = 20 / 10
			//Hypotenuse = distanceVar / cos(radians)  
			// 

			float adjustedDistance = distanceVar / cos(offsetAngleInRadians);

			// Unit Circle
			float3 rightPoint = (float3(cos(viewAngle + offsetAngleInRadians), 0, sin(viewAngle + offsetAngleInRadians)) * adjustedDistance) + basePoint;
			float3 leftPoint = (float3(cos(viewAngle - offsetAngleInRadians), 0, sin(viewAngle - offsetAngleInRadians)) * adjustedDistance) + basePoint;
			float3 centerPoint = (float3(cos(viewAngle), 0, sin(viewAngle)) * distanceVar) + basePoint;
			float3 pointInQuestion = IN.worldPos;

			c.rgb *= _MainColor;

			if(isPointInTheCircle(rightPoint.xz, pointInQuestion.xz, .3) ||
			   isPointInTheCircle(leftPoint.xz, pointInQuestion.xz, .3) ||
			   isPointInTheCircle(basePoint.xz, pointInQuestion.xz, .3) ||
			   isPointInTheCircle(centerPoint.xz, pointInQuestion.xz, .3))
			{
				o.Albedo = c.rgb * _FOVColor;
			}

			else
			{
				o.Albedo = c.rgb;
			}

			o.Alpha = c.a;
		}
		ENDCG
		}
			FallBack "Diffuse" //If we cannot use the subshader on specific hardware we will fallback to Diffuse shader
}
