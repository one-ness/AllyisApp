package com.allyis.timetracker.domain.access;

import org.json.*;
import org.json.simple.parser.*;
import org.json.simple.*;

import java.lang.String;
import java.util.Iterator;
import java.util.List;
import java.util.Map;

/**
 * Created by Jai on 7/14/14.
 */
/*public class LoginResponse extends BaseApiResponse {

    private String SessionToken = "{\"first\": 123, \"second\": [4, 5, 6], \"third\": 789}"; //can I just capitalize the SessionToken (issue??)

    JSONParser parser = new JSONParser();
    //Object obj = parser.parse(SessionToken);
    ContainerFactory containerFactory = new ContainerFactory() {
        @Override
        public Map createObjectContainer() {
            return null;
        }

        @Override
        public List creatArrayContainer() {
            return null;
        }
    };

    try

    {
        Map json = (Map) parser.parse(SessionToken, containerFactory);
        Iterator iter = json.entrySet().iterator();
        //System print line (to check results)
        while (iter.hasNext()) {
            Map.Entry entry = (Map.Entry) iter.next();

        }

    }
    catch(ParseException pe){
        System.out.println(pe);
    }

}

*/