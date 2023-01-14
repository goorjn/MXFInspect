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

using Myriadbits.MXF.Identifiers;
using Myriadbits.MXF.KLV;
using System;
using System.IO;
using System.Linq;
using static Myriadbits.MXF.KLV.KLVLength;
using static Myriadbits.MXF.KLVKey;

namespace Myriadbits.MXF
{
    public class MXFLocalTagParser : KLVTripletParser<MXFLocalTag, KLVKey, KLVLength>
    {
        public MXFLocalTagParser(Stream stream, long baseOffset) : base(stream, baseOffset)
        {
        }

        protected override KLVKey ParseKLVKey()
        {
            var keyLength = KeyLengths.TwoBytes;
            return new KLVKey(keyLength, reader.ReadBytes((int)keyLength));
        }

        protected override KLVLength ParseKLVLength()
        {
            var lengthEncoding = LengthEncodings.TwoBytes;
            return new KLVLength(lengthEncoding, reader.ReadBytes((int)lengthEncoding));
        }

        protected override MXFLocalTag InstantiateKLV(KLVKey key, KLVLength length, long offset, Stream stream)
        {
            return new MXFLocalTag(key, length, offset, stream);
        }
    }
}
