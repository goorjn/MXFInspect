#region license
//
// MXF - Myriadbits .NET MXF library. 
// Read MXF Files.
// Copyright (C) 2015 Myriadbits, Jochem Bakker
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
//
// For more information, contact me at: info@myriadbits.com
//
#endregion

using Myriadbits.MXF.Metadata;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Myriadbits.MXF
{

    /// <summary>
    /// Create the correct MXF (sub) object 
    /// </summary>
    public class MXFKLVFactory
    {
        static Dictionary<MXFKey, Type> dict = new Dictionary<MXFKey, Type>(new KeyPartialMatchComparer());
        static MXFKLVFactory()
        {
            #region Main Elements

            dict.Add(new MXFKey(0x06, 0x0e, 0x2b, 0x34, 0x02, 0x05, 0x01, 0x01, 0x0d, 0x01, 0x02, 0x01, 0x01, 0x02), typeof(MXFPartition));                 // Header partition
            dict.Add(new MXFKey(0x06, 0x0e, 0x2b, 0x34, 0x02, 0x05, 0x01, 0x01, 0x0d, 0x01, 0x02, 0x01, 0x01, 0x03), typeof(MXFPartition));                 // Body partition
            dict.Add(new MXFKey(0x06, 0x0e, 0x2b, 0x34, 0x02, 0x05, 0x01, 0x01, 0x0d, 0x01, 0x02, 0x01, 0x01, 0x04), typeof(MXFPartition));                 // Footer partition

            dict.Add(new MXFKey(0x06, 0x0e, 0x2b, 0x34, 0x02, 0x05, 0x01, 0x01, 0x0d, 0x01, 0x02, 0x01, 0x01, 0x05, 0x01, 0x00), typeof(MXFPrimerPack));

            dict.Add(new MXFKey(0x06, 0x0e, 0x2b, 0x34, 0x02, 0x05, 0x01, 0x01, 0x0d, 0x01, 0x02, 0x01, 0x01, 0x11, 0x01, 0x00), typeof(MXFRIP));

            dict.Add(new MXFKey(0x06, 0x0e, 0x2b, 0x34, 0x02, 0x05, 0x01, 0x01, 0x0d, 0x01, 0x03, 0x01, 0x04), typeof(MXFSystemItem));
            dict.Add(new MXFKey(0x06, 0x0e, 0x2b, 0x34, 0x02, 0x53, 0x01, 0x01, 0x0d, 0x01, 0x03, 0x01, 0x14), typeof(MXFSystemItem));

            dict.Add(new MXFKey(0x06, 0x0e, 0x2b, 0x34, 0x01, 0x02, 0x01, 0x01, 0x0d, 0x01, 0x03, 0x01, 0x05), typeof(MXFEssenceElement));
            dict.Add(new MXFKey(0x06, 0x0e, 0x2b, 0x34, 0x01, 0x02, 0x01, 0x01, 0x0d, 0x01, 0x03, 0x01, 0x06), typeof(MXFEssenceElement));
            dict.Add(new MXFKey(0x06, 0x0e, 0x2b, 0x34, 0x01, 0x02, 0x01, 0x01, 0x0d, 0x01, 0x03, 0x01, 0x07), typeof(MXFEssenceElement));
            dict.Add(new MXFKey(0x06, 0x0e, 0x2b, 0x34, 0x01, 0x02, 0x01, 0x01, 0x0d, 0x01, 0x03, 0x01, 0x15), typeof(MXFEssenceElement));
            dict.Add(new MXFKey(0x06, 0x0e, 0x2b, 0x34, 0x01, 0x02, 0x01, 0x01, 0x0d, 0x01, 0x03, 0x01, 0x16), typeof(MXFEssenceElement));
            dict.Add(new MXFKey(0x06, 0x0e, 0x2b, 0x34, 0x01, 0x02, 0x01, 0x01, 0x0d, 0x01, 0x03, 0x01, 0x18), typeof(MXFEssenceElement));

            // TODO disabled because key is not unique enough, adding to dictionary will thus cause exception.
            //dict.Add(new MXFKey(0x06, 0x0e, 0x2b, 0x34, 0x01, 0x02, 0x01, 0x01, 0x0e, 0x04, 0x03, 0x01),                       typeof(MXFAvidEssenceElement));

            // TODO cannot be found in SMPTE official registers ???
            // closest one: OrganizationallyRegisteredasPrivate 	http://www.smpte-ra.org/reg/400/2012/14 	urn:smpte:ul:060e2b34.04010101.0e000000.00000000
            dict.Add(new MXFKey("SonyMpeg4ExtraData", 0x06, 0x0e, 0x2b, 0x34, 0x04, 0x01, 0x01, 0x01, 0x0e, 0x06, 0x06, 0x02, 0x02, 0x01, 0x00, 0x00), typeof(MXFKLV));

            dict.Add(new MXFKey(0x06, 0x0e, 0x2b, 0x34, 0x01, 0x02, 0x01, 0x01, 0x0d, 0x01, 0x03, 0x01, 0x17), typeof(MXFANCFrameElement));

            dict.Add(new MXFKey(0x06, 0x0e, 0x2b, 0x34, 0x02, 0x43, 0x01, 0x01, 0x0d, 0x01, 0x03, 0x01, 0x04, 0x01), typeof(MXFPackageMetaData));
            dict.Add(new MXFKey(0x06, 0x0e, 0x2b, 0x34, 0x02, 0x63, 0x01, 0x01, 0x0d, 0x01, 0x03, 0x01, 0x04, 0x01), typeof(MXFPackageMetaData));

            dict.Add(new MXFKey(0x06, 0x0e, 0x2b, 0x34, 0x02, 0x53, 0x01, 0x01, 0x0d, 0x01, 0x02, 0x01, 0x01, 0x10, 0x01, 0x00), typeof(MXFIndexTableSegment));

            #endregion

            #region Main Elements

            dict.Add(new MXFKey(0x06, 0x0e, 0x2b, 0x34, 0x02, 0x53, 0x01, 0x01, 0x0d, 0x01, 0x04, 0x01, 0x02, 0x02, 0x00, 0x00), typeof(MXFCryptographicContext));

            dict.Add(new MXFKey(0x06, 0x0e, 0x2b, 0x34, 0x02, 0x53, 0x01, 0x01, 0x0d, 0x01, 0x01, 0x01, 0x01, 0x01, 0x2F, 0x00), typeof(MXFPreface));
            dict.Add(new MXFKey(0x06, 0x0e, 0x2b, 0x34, 0x02, 0x53, 0x01, 0x01, 0x0d, 0x01, 0x01, 0x01, 0x01, 0x01, 0x30, 0x00), typeof(MXFIdentification));
            dict.Add(new MXFKey(0x06, 0x0e, 0x2b, 0x34, 0x02, 0x53, 0x01, 0x01, 0x0d, 0x01, 0x01, 0x01, 0x01, 0x01, 0x18, 0x00), typeof(MXFContentStorage));
            dict.Add(new MXFKey(0x06, 0x0e, 0x2b, 0x34, 0x02, 0x53, 0x01, 0x01, 0x0d, 0x01, 0x01, 0x01, 0x01, 0x01, 0x23, 0x00), typeof(MXFEssenceContainerData));

            dict.Add(new MXFKey(0x06, 0x0e, 0x2b, 0x34, 0x02, 0x53, 0x01, 0x01, 0x0d, 0x01, 0x01, 0x01, 0x01, 0x01, 0x34, 0x00), typeof(MXFGenericPackage));
            dict.Add(new MXFKey(0x06, 0x0e, 0x2b, 0x34, 0x02, 0x53, 0x01, 0x01, 0x0d, 0x01, 0x01, 0x01, 0x01, 0x01, 0x36, 0x00), typeof(MXFMaterialPackage));
            dict.Add(new MXFKey(0x06, 0x0e, 0x2b, 0x34, 0x02, 0x53, 0x01, 0x01, 0x0d, 0x01, 0x01, 0x01, 0x01, 0x01, 0x37, 0x00), typeof(MXFSourcePackage));

            dict.Add(new MXFKey(0x06, 0x0e, 0x2b, 0x34, 0x02, 0x53, 0x01, 0x01, 0x0d, 0x01, 0x01, 0x01, 0x01, 0x01, 0x38, 0x00), typeof(MXFGenericTrack));
            dict.Add(new MXFKey(0x06, 0x0e, 0x2b, 0x34, 0x02, 0x53, 0x01, 0x01, 0x0d, 0x01, 0x01, 0x01, 0x01, 0x01, 0x39, 0x00), typeof(MXFEventTrack));
            dict.Add(new MXFKey(0x06, 0x0e, 0x2b, 0x34, 0x02, 0x53, 0x01, 0x01, 0x0d, 0x01, 0x01, 0x01, 0x01, 0x01, 0x3A, 0x00), typeof(MXFStaticTrack));
            dict.Add(new MXFKey(0x06, 0x0e, 0x2b, 0x34, 0x02, 0x53, 0x01, 0x01, 0x0d, 0x01, 0x01, 0x01, 0x01, 0x01, 0x3B, 0x00), typeof(MXFTimelineTrack));

            dict.Add(new MXFKey(0x06, 0x0e, 0x2b, 0x34, 0x02, 0x53, 0x01, 0x01, 0x0d, 0x01, 0x01, 0x01, 0x01, 0x01, 0x03, 0x00), typeof(MXFSegment));
            dict.Add(new MXFKey(0x06, 0x0e, 0x2b, 0x34, 0x02, 0x53, 0x01, 0x01, 0x0d, 0x01, 0x01, 0x01, 0x01, 0x01, 0x06, 0x00), typeof(MXFEvent));
            dict.Add(new MXFKey(0x06, 0x0e, 0x2b, 0x34, 0x02, 0x53, 0x01, 0x01, 0x0d, 0x01, 0x01, 0x01, 0x01, 0x01, 0x07, 0x00), typeof(MXFGPITrigger));
            dict.Add(new MXFKey(0x06, 0x0e, 0x2b, 0x34, 0x02, 0x53, 0x01, 0x01, 0x0d, 0x01, 0x01, 0x01, 0x01, 0x01, 0x0b, 0x00), typeof(MXFNestedScope));
            dict.Add(new MXFKey(0x06, 0x0e, 0x2b, 0x34, 0x02, 0x53, 0x01, 0x01, 0x0d, 0x01, 0x01, 0x01, 0x01, 0x01, 0x0d, 0x00), typeof(MXFScopeReference));
            dict.Add(new MXFKey(0x06, 0x0e, 0x2b, 0x34, 0x02, 0x53, 0x01, 0x01, 0x0d, 0x01, 0x01, 0x01, 0x01, 0x01, 0x0e, 0x00), typeof(MXFSelector));
            dict.Add(new MXFKey(0x06, 0x0e, 0x2b, 0x34, 0x02, 0x53, 0x01, 0x01, 0x0d, 0x01, 0x01, 0x01, 0x01, 0x01, 0x0F, 0x00), typeof(MXFSequence));
            dict.Add(new MXFKey(0x06, 0x0e, 0x2b, 0x34, 0x02, 0x53, 0x01, 0x01, 0x0d, 0x01, 0x01, 0x01, 0x01, 0x01, 0x10, 0x00), typeof(MXFSourceReference));
            dict.Add(new MXFKey(0x06, 0x0e, 0x2b, 0x34, 0x02, 0x53, 0x01, 0x01, 0x0d, 0x01, 0x01, 0x01, 0x01, 0x01, 0x11, 0x00), typeof(MXFSourceClip));
            dict.Add(new MXFKey(0x06, 0x0e, 0x2b, 0x34, 0x02, 0x53, 0x01, 0x01, 0x0d, 0x01, 0x01, 0x01, 0x01, 0x01, 0x14, 0x00), typeof(MXFTimecodeComponent));

            dict.Add(new MXFKey(0x06, 0x0e, 0x2b, 0x34, 0x02, 0x53, 0x01, 0x01, 0x0d, 0x01, 0x01, 0x01, 0x01, 0x01, 0x41, 0x00), typeof(MXFDescriptiveMarker));
            dict.Add(new MXFKey(0x06, 0x0e, 0x2b, 0x34, 0x02, 0x53, 0x01, 0x01, 0x0d, 0x01, 0x01, 0x01, 0x01, 0x01, 0x57, 0x00), typeof(MXFDynamicMarker));
            dict.Add(new MXFKey(0x06, 0x0e, 0x2b, 0x34, 0x02, 0x53, 0x01, 0x01, 0x0d, 0x01, 0x01, 0x01, 0x01, 0x01, 0x58, 0x00), typeof(MXFDynamicClip));
            dict.Add(new MXFKey(0x06, 0x0e, 0x2b, 0x34, 0x02, 0x53, 0x01, 0x01, 0x0d, 0x01, 0x01, 0x01, 0x01, 0x01, 0x60, 0x00), typeof(MXFDescriptiveClip));
            dict.Add(new MXFKey(0x06, 0x0e, 0x2b, 0x34, 0x02, 0x53, 0x01, 0x01, 0x0d, 0x01, 0x01, 0x01, 0x01, 0x01, 0x08, 0x00), typeof(MXFCommentMarker));

            dict.Add(new MXFKey(0x06, 0x0e, 0x2b, 0x34, 0x02, 0x53, 0x01, 0x01, 0x0d, 0x01, 0x01, 0x01, 0x01, 0x01, 0x09, 0x00), typeof(MXFFiller)); // Filler
            dict.Add(new MXFKey(0x06, 0x0e, 0x2b, 0x34, 0x01, 0x01, 0x01, 0x02, 0x03, 0x01, 0x02, 0x10, 0x01, 0x00, 0x00, 0x00), typeof(MXFFiller));
            dict.Add(new MXFKey(0x06, 0x0e, 0x2b, 0x34, 0x01, 0x01, 0x01, 0x01, 0x03, 0x01, 0x02, 0x10, 0x01, 0x00, 0x00, 0x00), typeof(MXFFiller)); // Old filler



            #endregion

            #region Descriptors

            dict.Add(new MXFKey(0x06, 0x0e, 0x2b, 0x34, 0x02, 0x53, 0x01, 0x01, 0x0d, 0x01, 0x01, 0x01, 0x01, 0x01, 0x25, 0x00), typeof(MXFFileDescriptor)); // File Descriptor

            dict.Add(new MXFKey(0x06, 0x0e, 0x2b, 0x34, 0x02, 0x53, 0x01, 0x01, 0x0d, 0x01, 0x01, 0x01, 0x01, 0x01, 0x27, 0x00), typeof(MXFGenericPictureEssenceDescriptor)); // Generic Picture Essence Descripto
            dict.Add(new MXFKey(0x06, 0x0e, 0x2b, 0x34, 0x02, 0x53, 0x01, 0x01, 0x0d, 0x01, 0x01, 0x01, 0x01, 0x01, 0x28, 0x00), typeof(MXFCDCIPictureEssenceDescriptor)); // CDCI Essence Descriptor
            dict.Add(new MXFKey(0x06, 0x0e, 0x2b, 0x34, 0x02, 0x53, 0x01, 0x01, 0x0d, 0x01, 0x01, 0x01, 0x01, 0x01, 0x51, 0x00), typeof(MXFMPEGPictureEssenceDescriptor)); // Descriptor: MPEG 2 Video
            dict.Add(new MXFKey(0x06, 0x0e, 0x2b, 0x34, 0x02, 0x53, 0x01, 0x01, 0x0d, 0x01, 0x01, 0x01, 0x01, 0x01, 0x29, 0x00), typeof(MXFRGBAPictureEssenceDescriptor)); // RGBA Essence Descriptor
            dict.Add(new MXFKey(0x06, 0x0e, 0x2b, 0x34, 0x02, 0x53, 0x01, 0x01, 0x0d, 0x01, 0x01, 0x01, 0x01, 0x01, 0x42, 0x00), typeof(MXFGenericSoundEssenceDescriptor)); // Generic Sound Essence Descriptor
            dict.Add(new MXFKey(0x06, 0x0e, 0x2b, 0x34, 0x02, 0x53, 0x01, 0x01, 0x0d, 0x01, 0x01, 0x01, 0x01, 0x01, 0x43, 0x00), typeof(MXFGenericDataEssenceDescriptor)); // Generic Data Essence Descriptor
            dict.Add(new MXFKey(0x06, 0x0e, 0x2b, 0x34, 0x02, 0x53, 0x01, 0x01, 0x0d, 0x01, 0x01, 0x01, 0x01, 0x01, 0x44, 0x00), typeof(MXFMultipleDescriptor)); // MultipleDescriptor

            dict.Add(new MXFKey(0x06, 0x0e, 0x2b, 0x34, 0x02, 0x53, 0x01, 0x01, 0x0d, 0x01, 0x01, 0x01, 0x01, 0x01, 0x31, 0x00), typeof(MXFLocator)); // Locator
            dict.Add(new MXFKey(0x06, 0x0e, 0x2b, 0x34, 0x02, 0x53, 0x01, 0x01, 0x0d, 0x01, 0x01, 0x01, 0x01, 0x01, 0x32, 0x00), typeof(MXFNetworkLocator)); // Network Locator
            dict.Add(new MXFKey(0x06, 0x0e, 0x2b, 0x34, 0x02, 0x53, 0x01, 0x01, 0x0d, 0x01, 0x01, 0x01, 0x01, 0x01, 0x33, 0x00), typeof(MXFTextLocator)); // Text Locator
            dict.Add(new MXFKey(0x06, 0x0e, 0x2b, 0x34, 0x02, 0x53, 0x01, 0x01, 0x0d, 0x01, 0x01, 0x01, 0x01, 0x01, 0x61, 0x00), typeof(MXFGenericDescriptor)); // Application Plug-in object
            dict.Add(new MXFKey(0x06, 0x0e, 0x2b, 0x34, 0x02, 0x53, 0x01, 0x01, 0x0d, 0x01, 0x01, 0x01, 0x01, 0x01, 0x62, 0x00), typeof(MXFGenericDescriptor)); // Application Referenced object

            dict.Add(new MXFKey(0x06, 0x0e, 0x2b, 0x34, 0x02, 0x53, 0x01, 0x01, 0x0d, 0x01, 0x01, 0x01, 0x01, 0x01, 0x47, 0x00), typeof(MXFAES3AudioEssenceDescriptor)); // Descriptor: AES3
            dict.Add(new MXFKey(0x06, 0x0e, 0x2b, 0x34, 0x02, 0x53, 0x01, 0x01, 0x0d, 0x01, 0x01, 0x01, 0x01, 0x01, 0x48, 0x00), typeof(MXFWaveAudioEssenceDescriptor)); // Descriptor: Wave
            dict.Add(new MXFKey(0x06, 0x0e, 0x2b, 0x34, 0x02, 0x53, 0x01, 0x01, 0x0d, 0x01, 0x01, 0x01, 0x01, 0x01, 0x5B, 0x00), typeof(MXFGenericDataEssenceDescriptor)); // Descriptor: VBI Data Descriptor, SMPTE 436 - 7.3
            dict.Add(new MXFKey(0x06, 0x0e, 0x2b, 0x34, 0x02, 0x53, 0x01, 0x01, 0x0d, 0x01, 0x01, 0x01, 0x01, 0x01, 0x5C, 0x00), typeof(MXFGenericDataEssenceDescriptor)); // Descriptor: ANC Data Descriptor, SMPTE 436 - 7.3

            // DCTimedTextDescriptor per SMPTE ST 429-5
            dict.Add(new MXFKey(0x06, 0x0e, 0x2b, 0x34, 0x02, 0x53, 0x01, 0x01, 0x0d, 0x01, 0x01, 0x01, 0x01, 0x01, 0x64, 0x00), typeof(MXFDCTimedTextDescriptor));

            #endregion

            #region Sub-Descriptors

            // ACESPictureSubDescriptor SMPTE ST 2067-50
            // urn:smpte:ul:060e2b34.027f0101.0d010101.01017900
            dict.Add(new MXFKey(0x06, 0x0e, 0x2b, 0x34, 0x02, 0x53, 0x01, 0x01, 0x0d, 0x01, 0x01, 0x01, 0x01, 0x01, 0x79, 0x00), typeof(ACESPictureSubDescriptor));

            // TargetFrameSubDescriptor SMPTE ST 2067-50
            dict.Add(new MXFKey(0x06, 0x0e, 0x2b, 0x34, 0x02, 0x53, 0x01, 0x01, 0x0d, 0x01, 0x01, 0x01, 0x01, 0x01, 0x7a, 0x00), typeof(TargetFrameSubDescriptor));

            // JPEG 2000 SubDescriptor per SMPTE ST 422 
            // urn:smpte:ul:060e2b34.027f0101.0d010101.01015a00
            dict.Add(new MXFKey(0x06, 0x0e, 0x2b, 0x34, 0x02, 0x53, 0x01, 0x01, 0x0d, 0x01, 0x01, 0x01, 0x01, 0x01, 0x5a, 0x00), typeof(JPEG2000SubDescriptor));

            // MCA Label SubDescriptors per SMPTE ST 377-4
            dict.Add(new MXFKey(0x06, 0x0e, 0x2b, 0x34, 0x02, 0x53, 0x01, 0x01, 0x0d, 0x01, 0x01, 0x01, 0x01, 0x01, 0x6a, 0x00), typeof(MCALabelSubDescriptor));
            dict.Add(new MXFKey(0x06, 0x0e, 0x2b, 0x34, 0x02, 0x53, 0x01, 0x01, 0x0d, 0x01, 0x01, 0x01, 0x01, 0x01, 0x6b, 0x00), typeof(AudioChannelLabelSubDescriptor));
            dict.Add(new MXFKey(0x06, 0x0e, 0x2b, 0x34, 0x02, 0x53, 0x01, 0x01, 0x0d, 0x01, 0x01, 0x01, 0x01, 0x01, 0x6c, 0x00), typeof(SoundfieldGroupLabelSubDescriptor));
            dict.Add(new MXFKey(0x06, 0x0e, 0x2b, 0x34, 0x02, 0x53, 0x01, 0x01, 0x0d, 0x01, 0x01, 0x01, 0x01, 0x01, 0x6d, 0x00), typeof(GroupOfSoundfieldGroupsLabelSubDescriptor));
            dict.Add(new MXFKey(0x06, 0x0e, 0x2b, 0x34, 0x02, 0x53, 0x01, 0x01, 0x0d, 0x01, 0x01, 0x01, 0x01, 0x01, 0x67, 0x00), typeof(MXFContainerConstraintsSubDescriptor));
            dict.Add(new MXFKey(0x06, 0x0e, 0x2b, 0x34, 0x02, 0x53, 0x01, 0x01, 0x0d, 0x01, 0x01, 0x01, 0x01, 0x01, 0x6e, 0x00), typeof(MXFAVCSubDescriptor));

            #endregion

            #region DescriptiveObjects

            dict.Add(new MXFKey(0x06, 0x0e, 0x2b, 0x34, 0x02, 0x53, 0x01, 0x01, 0x0d, 0x01, 0x04, 0x01, 0x04, 0x02, 0x01, 0x00), typeof(MXFGenericStreamTextBasedSet));
            dict.Add(new MXFKey(0x06, 0x0e, 0x2b, 0x34, 0x02, 0x53, 0x01, 0x01, 0x0d, 0x01, 0x04, 0x01, 0x04, 0x03, 0x01, 0x00), typeof(MXFTextBasedObject));

            // DescriptiveObject	
            // urn:smpte:ul:060e2b34.027f0101.0d010400.00000000
            dict.Add(new MXFKey(0x06, 0x0e, 0x2b, 0x34, 0x02, 0x53, 0x01, 0x01, 0x0d, 0x01, 0x04, 0x01, 0x00, 0x00, 0x00, 0x00), typeof(MXFDescriptiveObject));

            // Thesaurus
            // urn:smpte:ul:060e2b34.027f0101.0d010401.017f1200
            dict.Add(new MXFKey(0x06, 0x0e, 0x2b, 0x34, 0x02, 0x53, 0x01, 0x01, 0x0d, 0x01, 0x04, 0x01, 0x01, 0x7f, 0x12, 0x00), typeof(MXFThesaurus));

            // Contact 
            // urn:smpte:ul:060e2b34.027f0101.0d010401.017f1a00 
            dict.Add(new MXFKey(0x06, 0x0e, 0x2b, 0x34, 0x02, 0x53, 0x01, 0x01, 0x0d, 0x01, 0x04, 0x01, 0x01, 0x7f, 0x1a, 0x00), typeof(MXFContact));

            // Location
            // urn:smpte:ul:060e2b34.027f0101.0d010401.011a0400
            dict.Add(new MXFKey(0x06, 0x0e, 0x2b, 0x34, 0x02, 0x53, 0x01, 0x01, 0x0d, 0x01, 0x04, 0x01, 0x01, 0x1a, 0x04, 0x00), typeof(MXFLocation));

            // ContactsList
            // urn:smpte:ul:060e2b34.027f0101.0d010401.01190100
            dict.Add(new MXFKey(0x06, 0x0e, 0x2b, 0x34, 0x02, 0x53, 0x01, 0x01, 0x0d, 0x01, 0x04, 0x01, 0x01, 0x19, 0x01, 0x00), typeof(MXFContactsList));

            // Address
            // urn:smpte:ul:060e2b34.027f0101.0d010401.011b0100
            dict.Add(new MXFKey(0x06, 0x0e, 0x2b, 0x34, 0x02, 0x53, 0x01, 0x01, 0x0d, 0x01, 0x04, 0x01, 0x01, 0x1b, 0x01, 0x00), typeof(MXFAddress));

            #endregion

            #region DescriptiveFrameworks

            dict.Add(new MXFKey(0x06, 0x0e, 0x2b, 0x34, 0x02, 0x53, 0x01, 0x01, 0x0d, 0x01, 0x04, 0x01, 0x04, 0x01, 0x01, 0x00), typeof(MXFTextBasedFramework));

            // ProductionFramework 
            // urn:smpte:ul:060e2b34.027f0101.0d010401.01010100
            dict.Add(new MXFKey(0x06, 0x0e, 0x2b, 0x34, 0x02, 0x53, 0x01, 0x01, 0x0d, 0x01, 0x04, 0x01, 0x01, 0x01, 0x01, 0x00), typeof(MXFProductionFramework));

            // ProductionClipFramework 
            // urn:smpte:ul: 060e2b34.027f0101.0d010401.017f0200
            dict.Add(new MXFKey(0x06, 0x0e, 0x2b, 0x34, 0x02, 0x53, 0x01, 0x01, 0x0d, 0x01, 0x04, 0x01, 0x01, 0x7f, 0x02, 0x00), typeof(MXFProductionClipFramework));

            // DMS1Framework 
            // urn:smpte:ul:060e2b34.027f0101.0d010401.017f0100
            dict.Add(new MXFKey(0x06, 0x0e, 0x2b, 0x34, 0x02, 0x53, 0x01, 0x01, 0x0d, 0x01, 0x04, 0x01, 0x01, 0x7f, 0x01, 0x00), typeof(MXFDMS1Framework));
            #endregion

            #region No Groups

            // XMLDocumentText_Indirect
            // urn:smpte:ul:060e2b34.01010105.03010220.01000000
            dict.Add(new MXFKey(0x06, 0x0e, 0x2b, 0x34, 0x01, 0x01, 0x01, 0x05, 0x03, 0x01, 0x02, 0x20, 0x01, 0x00, 0x00, 0x00), typeof(MXFXMLDocumentText_Indirect));

            // ItemValue_ISO7
            //urn:smpte:ul:060e2b34.01010105.0301020a.02000000
            dict.Add(new MXFKey(0x06, 0x0e, 0x2b, 0x34, 0x01, 0x01, 0x01, 0x05, 0x03, 0x01, 0x02, 0x0a, 0x02, 0x00, 0x00, 0x00), typeof(MXFItemValue_ISO7));

            // LensUnitAcquisitionMetadata 
            // urn:smpte:ul:060e2b34.027f0101.0c020101.01010000
            dict.Add(new MXFKey(0x06, 0x0e, 0x2b, 0x34, 0x02, 0x53, 0x01, 0x01, 0x0c, 0x02, 0x01, 0x01, 0x01, 0x01, 0x00, 0x00), typeof(MXFLensUnitAquisitionMetadata));

            // CameraUnitAcquisitionMetadata 
            // urn:smpte:ul:060e2b34.027f0101.0c020101.02010000
            dict.Add(new MXFKey(0x06, 0x0e, 0x2b, 0x34, 0x02, 0x53, 0x01, 0x01, 0x0c, 0x02, 0x01, 0x01, 0x02, 0x01, 0x00, 0x00), typeof(MXFCameraUnitAquisitionMetadata));

            #endregion
        }


        /// <summary>
        /// Create a new MXF object based on the KLV key
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="currentPartition"></param>
        /// <returns></returns>
        public MXFKLV CreateObject(MXFReader reader, MXFPartition currentPartition)
        {

            MXFKLV klv = new MXFKLV(reader);
            klv.Partition = currentPartition; // Pass the current partition through to the classes

            if (dict.TryGetValue(klv.Key, out Type foundType))
            {
                return (MXFKLV)Activator.CreateInstance(foundType, reader, klv);
            }
            else
            {
                // TODO what if the key cannot be found, i.e. it is not known?
            }

            return klv;
        }


        /// <summary>
        /// Update all descriptions with data from the primer pack
        /// </summary>
        public static void UpdateAllTypeDescriptions(Dictionary<UInt16, MXFEntryPrimer> allPrimerKeys)
        {
            // Start by setting all properties of all these classes to readonly
            foreach (MXFKey key in dict.Keys)
            {
                UpdateTypeDescriptions(key.ObjectType, allPrimerKeys);
            }
        }


        /// <summary>
        /// Update description of type with data from the primer pack
        /// </summary>
        public static void UpdateTypeDescriptions(Type type, Dictionary<UInt16, MXFEntryPrimer> allPrimerKeys)
        {
            if (type != null && allPrimerKeys != null)
            {
                if (type.BaseType != null)
                    UpdateTypeDescriptions(type.BaseType, allPrimerKeys);

                foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(type))
                {
                    DescriptionAttribute attr = prop.Attributes[typeof(DescriptionAttribute)] as DescriptionAttribute;
                    if (attr != null)
                    {
                        if (!string.IsNullOrEmpty(attr.Description) && attr.Description.Length == 4)
                        {
                            string newDescription = "";

                            // Get the local tag
                            try
                            {
                                UInt16 localTag = (UInt16)Convert.ToInt32(attr.Description, 16);

                                // Find the local tag in the primer pack
                                if (allPrimerKeys.ContainsKey(localTag))
                                {
                                    MXFEntryPrimer prime = allPrimerKeys[localTag];
                                    newDescription = prime.AliasUID.Key.Name;
                                }

                                FieldInfo fi = attr.GetType().GetField("description", BindingFlags.NonPublic | BindingFlags.Instance);
                                if (fi != null)
                                    fi.SetValue(attr, newDescription);
                            }
                            catch (Exception)
                            {

                            }
                        }
                    }
                }
            }
        }

        // TODO should it be public or internal?
        public class KeyPartialMatchComparer : IEqualityComparer<MXFKey>
        {
            // if the keys to compare are of the same category (meaning the same hash) compare
            // whether the byte sequence is equal
            public bool Equals(MXFKey x, MXFKey y)
            {
                return x.HasSameBeginning(y);
            }

            public int GetHashCode(MXFKey obj)
            {
                // hash only the first 5 bytes (prefix is 4 bytes + 5th byte = key category)
                return obj.GetHashCode(5);
            }
        }
    }
}
