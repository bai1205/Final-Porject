Shader "Unlit/DepthShader" { //�����Ⱦ�������ڸ���ͼ����ÿһ���������ֵ
    SubShader{
        // ������Ⱦ��ǩ����ʾ���shader�����ڲ�͸��������
        Tags { "RenderType" = "Opaque" }

        // ����Pass���֣�һ��shader���԰������Pass
        Pass{
            // ��ʼCG����Σ�CG��C for Graphics��һ����NVIDIA�����ĸ߼�ͼ����Ⱦ����
            CGPROGRAM
            // ָ��vertex shader����Ϊ"vert"
            #pragma vertex vert  
            // ָ��fragment shader����Ϊ"frag"
            #pragma fragment frag  
            // ָ��fragment shader����Ϊ"frag"
            #include "UnityCG.cginc"  
                
            // ��������������Ĳ�����
            sampler2D _CameraDepthTexture;
            
            // ����vertex��fragment�����ݽṹ
            struct v2f {
               // SV_POSITION��һ�����壬��ʾ���Ƕ������Ļλ��
               float4 pos : SV_POSITION;
               // TEXCOORD1��ʾ����һ����������
               float4 scrPos:TEXCOORD1;
            };

            // Vertex Shader����
            v2f vert(appdata_base v) {
               v2f o;
               // ������ռ�Ķ���λ��ת��Ϊ�ü��ռ��λ��
               o.pos = UnityObjectToClipPos(v.vertex);
               // ������Ļλ��
               o.scrPos = ComputeScreenPos(o.pos);
               return o;
            }

            // Fragment Shader����
            half4 frag(v2f i) : COLOR{
               // ��_CameraDepthTexture�����в������ֵ��Ȼ����ת��Ϊ�����Ӿ����
               float depthValue = LinearEyeDepth(tex2Dproj(_CameraDepthTexture,UNITY_PROJ_COORD(i.scrPos)).r);
               // �������ֵ��ΪRGB��ͨ����ֵ��͸����Ϊ1
               return half4(depthValue,depthValue,depthValue,1);
            }
            // ����CG�����
            ENDCG
        }
    }
        // �����ǰ����ȾӲ����֧�����shader����ôʹ��Diffuse��Ϊ��ѡ����
        FallBack "Diffuse"
}
