import React, { useEffect, useState } from "react";
import ReactSelect from "react-select";
import { assignUserToProcedure } from '../../../api/api'

const PlanProcedureItem = ({ procedure, users }) => {
    const [selectedUsers, setSelectedUsers] = useState(null);
    // console.log(`the procedure ${JSON.stringify(procedure)}`);
    // console.log(`the users are ${JSON.stringify(users)}`);

    // from the users array let's get the users who are assinged and display it
    useEffect(() => {
        if (procedure.userPlanProcedureMapping) {
            const assingedUsers = users.filter(item => {
                return procedure.userPlanProcedureMapping.some(user => user.userId == item.value)
            });
            setSelectedUsers(assingedUsers);
        }

    }, []);
    const handleAssignUserToProcedure = async (e) => {
        setSelectedUsers(e);
        debugger
        console.log(e);
        const lastSelectedOption = e[e.length - 1];
        
        if ((selectedUsers == null) || (e.length > selectedUsers.length)) {
            await assignUserToProcedure(procedure.planId, procedure.procedureId,lastSelectedOption.value);
        }
    };

    return (
        <div className="py-2">
            <div>
                {procedure.procedure.procedureTitle}
            </div>

            <ReactSelect
                className="mt-2"
                placeholder="Select User to Assign"
                isMulti={true}
                options={users}
                value={selectedUsers}
                onChange={(e) => { handleAssignUserToProcedure(e) }
                }
            />
        </div>
    );
};

export default PlanProcedureItem;
