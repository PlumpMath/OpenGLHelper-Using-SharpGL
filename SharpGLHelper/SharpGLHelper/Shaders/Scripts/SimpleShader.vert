#version 330 core
 
in vec4 Position;
in vec3 Normal;

uniform mat4 Projection;
uniform mat4 Modelview;
uniform mat3 NormalMatrix;
uniform vec3 DiffuseMaterial;
uniform mat4 TransformationMatrix;

out vec3 EyespaceNormal;
out vec3 Diffuse;

void main()
{

	vec4 pos;
	mat4 transformation = mat4(1.0);

	for (int i = 0; i < 4; i++)
	{
		float newPosVal = 0.0;
		for (int j = 0; j < 4; j++) 
		{
			newPosVal += Position[j] * TransformationMatrix[j][i];
		}
		pos[i] = newPosVal;
	}

    EyespaceNormal = NormalMatrix * Normal;
    gl_Position = Projection * Modelview * pos;
    Diffuse = DiffuseMaterial;
}