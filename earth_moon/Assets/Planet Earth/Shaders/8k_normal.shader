Shader "Custom/8k_Normal" {
	Properties {  
        _MainTex0 ("Top Left Texture", 2D) = "white" {}  
        //Added three more textures slots, one for each image  
        _MainTex1 ("Top Right Texture", 2D) = "white" {}  
        _MainTex2 ("Bottom Left Texture", 2D) = "white" {}  
        _MainTex3 ("Bottom Right Texture", 2D) = "white" {} 
        
        _Normals("_Normals", 2D) = "black" {}
    }  
    SubShader {  
        Tags { "RenderType"="Opaque" }  
        LOD 200  
  
        CGPROGRAM  
        #pragma surface surf Lambert  
  
        sampler2D _MainTex0;  
        //Added three more 2D samplers, one for each additional texture  
        sampler2D _MainTex1;  
        sampler2D _MainTex2;  
        sampler2D _MainTex3;  
  		sampler2D _Normals;
        struct Input {  
            float2 uv_MainTex0;  
            float2 uv_Normals;
        };  
  
        //this variable stores the current texture coordinates multiplied by 2  
        float2 dbl_uv_MainTex0;  
  
        void surf (Input IN, inout SurfaceOutput o) {  
  
            //multiply the current vertex texture coordinate by two  
            dbl_uv_MainTex0 = IN.uv_MainTex0*2;  
  
            //add an offset to the texture coordinates for each of the input textures  
            half4 c0 = tex2D (_MainTex0, dbl_uv_MainTex0 - float2(0.0, 1.0));  
            half4 c1 = tex2D (_MainTex1, dbl_uv_MainTex0 - float2(1.0, 1.0));  
            half4 c2 = tex2D (_MainTex2, dbl_uv_MainTex0);  
            half4 c3 = tex2D (_MainTex3, dbl_uv_MainTex0 - float2(1.0, 0.0));  
  
            //this if statement assures that the input textures won't overlap  
            if(IN.uv_MainTex0.x >= 0.5)  
            {  
                if(IN.uv_MainTex0.y <= 0.5)  
                {  
                    c0.rgb = c1.rgb = c2.rgb = 0;  
                }  
                else  
                {  
                    c0.rgb = c2.rgb = c3.rgb = 0;  
                }  
            }  
            else  
            {  
                if(IN.uv_MainTex0.y <= 0.5)  
                {  
                    c0.rgb = c1.rgb = c3.rgb = 0;  
                }  
                else  
                {  
                    c1.rgb = c2.rgb = c3.rgb = 0;  
                }  
            }  
  			float4 Sampled2D0=tex2D(_Normals,IN.uv_Normals.xy);
        	float4 UnpackNormal0=float4(UnpackNormal(Sampled2D0).xyz, 1.0);
            //sum the colors and the alpha, passing them to the Output Surface 'o'  
            o.Albedo = c0.rgb + c1.rgb + c2.rgb + c3.rgb;  
            
            o.Normal = UnpackNormal0;
            
            o.Alpha = c0.a + c1.a + c2.a + c3.a ;  
            
        }  
        ENDCG  
    }  
    FallBack "Diffuse"  
}  