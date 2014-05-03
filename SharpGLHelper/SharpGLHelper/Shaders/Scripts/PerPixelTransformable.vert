#version 130
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
	vec4 normalVec4 = vec4(Normal, 1.0);
	mat4 transformation = mat4(1.0);

	for (int i = 0; i < 4; i++)
	{
		float newPosVal = 0.0;
		float newNormalVal = 0.0;
		for (int j = 0; j < 4; j++) 
		{
			newPosVal += Position[j] * TransformationMatrix[j][i];
			newNormalVal += Normal[j] * TransformationMatrix[j][i];
		}
		pos[i] = newPosVal;
		normalVec4[i] = newNormalVal;
	}

	vec3 normal = vec3(normalVec4.x,normalVec4.y,normalVec4.z);

	// Uncomment to let the camera angle act as light direction.
	/*mat3 normalMatrix;
	normalMatrix[0] = Modelview[0].xyz;
	normalMatrix[1] = Modelview[1].xyz;
	normalMatrix[2] = Modelview[2].xyz;

    EyespaceNormal = normalMatrix * normal;*/

    EyespaceNormal = NormalMatrix * normal;
    gl_Position = Projection * Modelview * pos;
    Diffuse = DiffuseMaterial;
}