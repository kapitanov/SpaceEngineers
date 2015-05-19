﻿using Sandbox.Common.ObjectBuilders;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sandbox.Game.Entities.Blocks
{
    partial class MyTextPanel : Sandbox.ModAPI.Ingame.IMyTextPanel
    {
        private StringBuilder m_publicTitleHelper = new StringBuilder();
        private StringBuilder m_privateTitleHelper = new StringBuilder();
        private StringBuilder m_publicDescriptionHelper = new StringBuilder();
        private StringBuilder m_privateDescriptionHelper = new StringBuilder();

        string ModAPI.Ingame.IMyTextPanel.Font { get { return Font; } }

        void ModAPI.Ingame.IMyTextPanel.GetFonts(List<string> ids)
        {
            foreach (var definition in m_fontDefinitions)
            {
                ids.Add(definition.Id.SubtypeName);
            }
        }

        void ModAPI.Ingame.IMyTextPanel.SetFont(string id)
        {
            Font = id;
        }

        void ModAPI.Ingame.IMyTextPanel.ShowPublicTextOnScreen()
        {
            SyncObject.SendShowOnScreenChangeRequest((byte)ShowTextOnScreenFlag.PUBLIC);
        }

        void ModAPI.Ingame.IMyTextPanel.ShowPrivateTextOnScreen()
        {
            SyncObject.SendShowOnScreenChangeRequest((byte)ShowTextOnScreenFlag.PRIVATE);
        }

        void ModAPI.Ingame.IMyTextPanel.ShowTextureOnScreen()
        {
            SyncObject.SendShowOnScreenChangeRequest((byte)ShowTextOnScreenFlag.NONE);
        }

        void ModAPI.Ingame.IMyTextPanel.SetShowOnScreen(ShowTextOnScreenFlag set)
        {
            SyncObject.SendShowOnScreenChangeRequest((byte)set);
        }

        bool ModAPI.Ingame.IMyTextPanel.WritePublicTitle(string value, bool append)
        {
            if (m_isOpen)
            {
                return false;
            }
            if (append == false)
            {
                m_publicTitleHelper.Clear();
            }
            m_publicTitleHelper.Append(value);
            SyncObject.SendChangeTitleMessage(m_publicTitleHelper, true);
            return true;
        }

        string ModAPI.Ingame.IMyTextPanel.GetPublicTitle()
        {
            return m_publicTitle.ToString();
        }

        bool ModAPI.Ingame.IMyTextPanel.WritePublicText(string value, bool append)
        {
            if (m_isOpen)
            {
                return false;
            }
            if (append == false)
            {
                m_publicDescriptionHelper.Clear();
            }
            m_publicDescriptionHelper.Append(value);
            SyncObject.SendChangeDescriptionMessage(m_publicDescriptionHelper, true);
            return true;
        }

        string ModAPI.Ingame.IMyTextPanel.GetPublicText()
        {
            return m_publicDescription.ToString();
        }

        bool ModAPI.Ingame.IMyTextPanel.WritePrivateTitle(string value, bool append)
        {
            if (m_isOpen)
            {
                return false;
            }
            if (append == false)
            {
                m_privateTitleHelper.Clear();
            }
            m_privateTitleHelper.Append(value);
            SyncObject.SendChangeTitleMessage(m_privateTitleHelper, false);
            return true;
        }

        string ModAPI.Ingame.IMyTextPanel.GetPrivateTitle()
        {
            return m_privateTitle.ToString();
        }

        bool ModAPI.Ingame.IMyTextPanel.WritePrivateText(string value, bool append)
        {
            if (m_isOpen)
            {
                return false;
            }
            if (append == false)
            {
                m_privateDescriptionHelper.Clear();
            }
            m_privateDescriptionHelper.Append(value);
            SyncObject.SendChangeDescriptionMessage(m_privateDescriptionHelper, false);
            return true;
        }

        string ModAPI.Ingame.IMyTextPanel.GetPrivateText()
        {
            return m_privateDescription.ToString();
        }

        void ModAPI.Ingame.IMyTextPanel.AddImageToSelection(string id, bool checkExistence)
        {
            if (id == null)
            {
                return;
            }
            for (int t = 0; t < m_definitions.Count; t++)
            {
                if (m_definitions[t].Id.SubtypeName == id)
                {
                    if (checkExistence)
                    {
                        for (int i = 0; i < m_selectedTexturesToDraw.Count; i++)
                        {
                            if (m_selectedTexturesToDraw[i].Id.SubtypeName == id)
                                return;
                        }
                    }
                    SyncObject.SendAddImagesToSelectionRequest(new int[1] { t });
                    return;
                }
            }
        }

        void ModAPI.Ingame.IMyTextPanel.AddImagesToSelection(List<string> ids, bool checkExistence)
        {
            if (ids == null)
            {
                return;
            }
            List<int> selection = new List<int>();
            bool doNotAdd;
            foreach (var id in ids)
            {
                for (int t = 0; t < m_definitions.Count; t++)
                {
                    if (m_definitions[t].Id.SubtypeName == id)
                    {
                        doNotAdd = false;
                        if (checkExistence)
                        {
                            for (int i = 0; i < m_selectedTexturesToDraw.Count; i++)
                            {
                                if (m_selectedTexturesToDraw[i].Id.SubtypeName == id)
                                {
                                    doNotAdd = true;
                                    break;
                                }
                            }
                        }
                        if (!doNotAdd)
                        {
                            selection.Add(t);
                        }
                        break;
                    }
                }
            }
            if (selection.Count > 0)
            {
                SyncObject.SendAddImagesToSelectionRequest(selection.ToArray());
            }
        }

        void ModAPI.Ingame.IMyTextPanel.RemoveImageFromSelection(string id, bool removeDuplicates)
        {
            if (id == null)
            {
                return;
            }
            List<int> selection = new List<int>();
            for (int t = 0; t < m_definitions.Count; t++)
            {
                if (m_definitions[t].Id.SubtypeName == id)
                {
                    if (removeDuplicates)
                    {
                        for (int s = 0; s < m_selectedTexturesToDraw.Count; s++)
                        {
                            if (m_selectedTexturesToDraw[s].Id.SubtypeName == id)
                            {
                                selection.Add(t);
                            }
                        }
                    }
                    else
                    {
                        selection.Add(t);
                    }
                    break;
                }
            }
            if (selection.Count > 0)
            {
                SyncObject.SendRemoveSelectedImageRequest(selection.ToArray());
            }
        }

        void ModAPI.Ingame.IMyTextPanel.RemoveImagesFromSelection(List<string> ids, bool removeDuplicates)
        {
            if (ids == null)
            {
                return;
            }
            List<int> selection = new List<int>();
            foreach (var id in ids)
            {
                for (int t = 0; t < m_definitions.Count; t++)
                {
                    if (m_definitions[t].Id.SubtypeName == id)
                    {
                        if (removeDuplicates)
                        {
                            for (int s = 0; s < m_selectedTexturesToDraw.Count; s++)
                            {
                                if (m_selectedTexturesToDraw[s].Id.SubtypeName == id)
                                {
                                    selection.Add(t);
                                }
                            }
                        }
                        else
                        {
                            selection.Add(t);
                        }
                        break;
                    }
                }
            }
            if (selection.Count > 0)
            {
                SyncObject.SendRemoveSelectedImageRequest(selection.ToArray());
            }
        }

        void ModAPI.Ingame.IMyTextPanel.ClearImagesFromSelection()
        {
            if (m_selectedTexturesToDraw.Count == 0)
            {
                return;
            }
            List<int> selection = new List<int>();
            for (int i = 0; i < m_selectedTexturesToDraw.Count; i++)
            {
                for (int t = 0; t < m_definitions.Count; t++)
                {
                    if (m_definitions[t].Id.SubtypeName == m_selectedTexturesToDraw[i].Id.SubtypeName)
                    {
                        selection.Add(t);
                        break;
                    }
                }
            }
            SyncObject.SendRemoveSelectedImageRequest(selection.ToArray());
        }
    }
}
