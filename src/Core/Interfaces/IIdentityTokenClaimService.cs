﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EStore.Core.Interfaces;
public interface IIdentityTokenClaimService
{
  Task<string> GetTokenAsync(string userName);

}
