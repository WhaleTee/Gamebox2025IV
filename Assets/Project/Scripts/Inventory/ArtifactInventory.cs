using System.Collections.Generic;
using UnityEngine;

namespace Artifacts
{
    public class ArtifactInventory
    {
        private readonly List<Artifact> artifacts = new List<Artifact>();

        public void AddArtifact(Artifact artifact)
        {
            if (!artifacts.Contains(artifact)) artifacts.Add(artifact);
        }

        public void RemoveArtifact(Artifact artifact)
        {
            artifacts.Remove(artifact);
            TrimInventory();
        }

        public void RemoveArtifact(int index)
        {
            if (index < 0 || index >= artifacts.Count) return;
            artifacts.RemoveAt(index);
            TrimInventory();
        }

        public Artifact GetArtifact(int index)
        {
            if (--index < 0 || index >= artifacts.Count) return null;
            return artifacts[index];
        }

        private void TrimInventory()
        {
            var array = artifacts.ToArray();
            artifacts.Clear();
            foreach (var a in array)
            {
                artifacts.Add(a);
            }
        }
    }
}