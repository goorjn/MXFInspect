﻿#region license
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

using System;
using System.ComponentModel;

namespace Myriadbits.MXF
{
	public class MXFEvent : MXFSegment
	{
		[CategoryAttribute("Event"), Description("0601")]
		public UInt64? EventStartPosition { get; set; }
		[CategoryAttribute("Event"), Description("0602")]
		public string EventComment { get; set; }

		public MXFEvent(MXFReader reader, MXFKLV headerKLV, string metadataName)
			: base(reader, headerKLV, "Event")
		{
		}

		/// <summary>
		/// Overridden method to process local tags
		/// </summary>
		/// <param name="localTag"></param>
		protected override bool ParseLocalTag(MXFReader reader, MXFLocalTag localTag)
		{
			switch (localTag.Tag)
			{
				case 0x0601: this.EventStartPosition = reader.ReadUInt64(); return true;
				case 0x0602: this.EventComment = reader.ReadUTF16String(localTag.Size); return true;
			}
			return base.ParseLocalTag(reader, localTag); 
		}

	}
}
