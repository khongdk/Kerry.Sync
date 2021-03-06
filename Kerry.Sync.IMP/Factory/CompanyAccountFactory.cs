﻿using Kerry.Sync.IMP.Common;
using Kerry.Sync.IMP.Constants;
using Kerry.Sync.IMP.Model;
using Kerry.Sync.Utility.DB;
using Kerry.Sync.Utility.TaskManger;
using Kerry.Sync.Utility.Text;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Kerry.Sync.IMP
{
    public  class CompanyAccountFactory: BaseFactory
    {
        #region  Sql Part
        public override string GetK3Data(StringBuilder sb)
        {
            return string.Format(@"
                        SELECT P.PARTYID COMPANYCODE, A.SNO,A.BIZTYPE,A.CURRCODE,A.ACCOUNTTYPE,ACNO MAPCODE, OWNERID STATIONCODE FROM FMPARTY P 
                            INNER JOIN FMPARTYACC A 
                            ON P.PARTYID = A.PARTYID
                             WHERE P.STATUS = 'C' 
                        and P.PARTYID IN ({0})
                    ", sb.ToString());
        }

        public override string InitialInsertStr()
        {
            var initialInser = @"insert ignore into tb_company_account 
                                (company_id,sno,biztype,currency,account_type,mapcode,station_code,create_by,
                                update_by,CREATE_TIMESTAMP,UPDATE_TIMESTAMP) values ";
           
            return initialInser;
        }

        public override StringBuilder InsertK3Data(StringBuilder insertStr, DataRow r)
        {
            insertStr = insertStr.Append(string.Format("({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10}),",
            "(select id from tb_company where company_code = '" + r["COMPANYCODE"] + "' limit 1)", "'" + r["SNO"] + "'", "'" + r["BIZTYPE"] + "'", "'" + r["CURRCODE"] + "'", "'" + r["ACCOUNTTYPE"] + "'",
            "'" + r["MAPCODE"] + "'", "'" + r["STATIONCODE"] + "'", "'K3PATCH'", "'K3PATCH'", "sysdate()", "sysdate()"));
            return insertStr;
        }
        #endregion

    }




}
