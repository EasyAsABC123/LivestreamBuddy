﻿/*Copyright (C) 2013 Robert A. Boucher Jr. (TuFFrabit)

This library is free software; you can redistribute it and/or
modify it under the terms of the GNU Lesser General Public
License as published by the Free Software Foundation; either
version 2.1 of the License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public
License along with this library; if not, write to the Free Software
Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LobsterKnifeFight
{
    public class User
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Password { get; set; }

        public Dictionary<UserScope, AccessToken> AccessTokens { get; set; }

        public AccessToken GetAccessToken(UserScope scope)
        {
            AccessToken accessToken = null;

            if (this.AccessTokens != null)
            {
                if (this.AccessTokens.ContainsKey(scope))
                {
                    accessToken = this.AccessTokens[scope];
                }
            }

            return accessToken;
        }

        public string AccessToken { get; set; }

        public User()
        {
            this.AccessTokens = new Dictionary<UserScope, AccessToken>();
        }
    }
}
