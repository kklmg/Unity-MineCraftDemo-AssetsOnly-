Shader "Unlit/unlitsh"
{
    Properties
    {
		/*_MainColor("Main Color", Color) = (1,.1,.5,1)
		_SpecColor("high light", Color) = (1,1,1,1)
		_Emission("emission", Color) = (0,0,0,0)
		_Shininess("shiness", Range(0.01, 1)) = 0.7*/

		_MainTex("texture",2D) = "White"{TexGen SphereMap}
		//_BlendTex("blend(RGBA) ", 2D) = "white" {}
    }
    SubShader
	{

		//Tags{ "Queue" = "Transparent" }
		//Tags{ "Queue" = "Transparent+1" }

		Pass
		{

			//---------------------------Material---------------------------------//
			Material
			{
				//Diffuse[_MainColor]
				//Ambient[_MainColor]
				/*Shininess[_Shininess]
				Specular[_SpecColor]
				Emission[_Emission]*/


			}

			//-------------------------Lighting---------------------------------//

			Lighting on

			//SeparateSpecular On


			//-------------------------Texture ---------------------------------//
		
			/*SetTexture[_MainTex]
			{
				constantColor(1,1,1,1)
				combine constant lerp(texture) previous
			}

			SetTexture[_MainTex]
			{
				combine previous * texture
			}*/
			SetTexture[_MainTex]{ combine texture }		
			//SetTexture[_BlendTex]{ combine texture * previous }
			//SetTexture[_MainTex]{ Combine texture * primary DOUBLE, texture * primary }

		}

		
	}

	Fallback" Diffuse"
}
