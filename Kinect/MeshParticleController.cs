using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class MeshParticleController : MonoBehaviour
{
    public ParticleSystem pSystem;
    public ParticleSystem.Particle[] particle;

    public int divi = 1;
    public int particleSpeed = 4;

    public SkinnedMeshRenderer meshRenderer;
    private Mesh body;
    private Vector3[] vertices;

    private void Start()
    {
        //Create emty Mesh
        body = new Mesh();

        //Get Particle System
        pSystem = GetComponent<ParticleSystem>();

        //Get Skinned Mesh Renderer and save snapeshot from the Mesh
        meshRenderer = GetComponentInParent<SkinnedMeshRenderer>();
        meshRenderer.BakeMesh(body);

        //Get position from the Vertices of the Mesh
        vertices = body.vertices;

        //Set max. Particle to the count oft the Vertices. Option: divi for a multiple of the Vertices
        var main = pSystem.main;
        main.maxParticles = vertices.Length / divi;
    }

    private void LateUpdate()
    {
        //Get ParticleSystem and Particles
        particle = new ParticleSystem.Particle[pSystem.main.maxParticles];
        pSystem.GetParticles(particle);
        
        //Get Skinned Mesh Renderer and the Mesh
        meshRenderer = GetComponentInParent<SkinnedMeshRenderer>();
        meshRenderer.BakeMesh(body);

        //Get position from the Vertices of the Mesh
        vertices = body.vertices;

        moveParticle();

        //Set the changed Particles to the Particle System
        pSystem.SetParticles(particle, pSystem.main.maxParticles);
    }

    private void moveParticle()
    {
        for (int i = 0; i < particle.Length; i++)
        {
            //Old!!! Test version
            //particle[i].position = transform.TransformPoint(vertices[i * divi]);

            //Particle moves to there Vertex Position. Option: ParticleSpeed for the speed for a specific distance
            if (Vector3.Distance(particle[i].position, vertices[i * divi]) > particleSpeed)
                particle[i].velocity = (vertices[i * divi] - particle[i].position) * Vector3.Distance(particle[i].position, vertices[i * divi]);
            else
                particle[i].velocity = (vertices[i * divi] - particle[i].position) * particleSpeed;
        }
    }
}
