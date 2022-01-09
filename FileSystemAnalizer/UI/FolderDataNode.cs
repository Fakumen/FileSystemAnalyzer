﻿using FileSystemAnalizer.App;
using FileSystemAnalizer.Domain;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileSystemAnalizer.UI
{
    public class FolderDataNode : TreeNode, IFolderDataNode
    {
        public IFolderScanData ScanData { get; }
        public string Label
        {
            get => Text;
            set => Text = value;
        }

        public IEnumerable<IFolderDataNode> FolderDataNodes => folderNodes;

        public IEnumerable<IFileDataNode> FileDataNodes => fileNodes;

        private readonly List<FolderDataNode> folderNodes = new List<FolderDataNode>();
        private readonly List<FileDataNode> fileNodes = new List<FileDataNode>();

        public FolderDataNode(IFolderScanData folderData)
        {
            ScanData = folderData;
            var sizeUnits = ScanData.Size.BestFittingUnits;
            Text = $"{ScanData.Name} [{ScanData.Size.GetInUnits(sizeUnits):f1} {sizeUnits.ToString()}]";
            SelectedImageKey = ImageKey = IconPool.FolderIconKey;
        }

        public void AddNode(IFileDataNode fileNode)
        {
            var castedNode = (FileDataNode)fileNode;
            fileNodes.Add(castedNode);
            Nodes.Add(castedNode);
        }

        public void AddNode(IFolderDataNode folderNode)
        {
            var castedNode = (FolderDataNode)folderNode;
            folderNodes.Add(castedNode);
            Nodes.Add(castedNode);
        }

        public void ClearNodes()
        {
            Nodes.Clear();
            fileNodes.Clear();
            folderNodes.Clear();
        }

        public void CreateAllSubNodes()
        {
            foreach (var folderData in ScanData.Folders)
            {
                var folderNode = new FolderDataNode(folderData);
                AddNode(folderNode);
                folderNode.CreateAllSubNodes();
            }
            foreach (var fileData in ScanData.Files)
            {
                var fileNode = new FileDataNode(fileData);
                AddNode(fileNode);
            }
        }
    }
}
