#version 330 core
in int PositionNumber;
in mat4 TransformationMatrix;
in vec3 DiffuseMaterial;

uniform mat4 Projection;
uniform mat4 Modelview;

out vec3 Diffuse;

void main()
{
	const vec3 positions[8] = vec3[8](
		vec3(1,1,1),
		vec3(1,1,-1),
		vec3(1,-1,1),
		vec3(1,-1,-1),
		vec3(-1,1,1),
		vec3(-1,1,-1),
		vec3(-1,-1,1)
		vec3(-1,-1,-1)
	);
	vec4 pos = positions[PositionNumber];
	mat4 transformation = mat4(1.0);

	for (int i = 0; i < 4; i++)
	{
		float newPosVal = 0.0;
		for (int j = 0; j < 4; j++) 
		{
			newPosVal += pos[j] * TransformationMatrix[j][i];
		}
		pos[i] = newPosVal;
	}

    gl_Position = Projection * Modelview * pos;
    Diffuse = DiffuseMaterial;
}