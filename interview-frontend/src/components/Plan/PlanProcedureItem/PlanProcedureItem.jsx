import React, { useEffect, useState } from "react";
import ReactSelect from "react-select";
import { assignUserToProcedure, removeUserFromProcedure } from '../../../api/api'

const PlanProcedureItem = ({ procedure, users }) => {
    const [selectedUsers, setSelectedUsers] = useState(null);
    // from the users array let's get the users who are assinged and display it
    useEffect(() => {
        if (procedure.userPlanProcedureMapping) {
            const assingedUsers = users.filter(item => {
                return procedure.userPlanProcedureMapping.some(user => user.userId === item.value)
            });
            setSelectedUsers(assingedUsers);
        }

    }, []);
    const handleAssignUserToProcedure = async (e) => {
        setSelectedUsers(e);
        const lastSelectedOption = e[e.length - 1];
        if ((selectedUsers === null) || (e.length > selectedUsers.length)) {
            await assignUserToProcedure(procedure.planId, procedure.procedureId,lastSelectedOption.value);
        }
        else {
            let removedUserId = null; // if this is null then we are removing all users from the planprocedure
            if (e.length > 0) {
                const removedUser = findRemovedUser(selectedUsers,e);
                removedUserId = removedUser.value;
            }
            await removeUserFromProcedure(procedure.planId, procedure.procedureId, removedUserId);
        }
    };

    function findRemovedUser(originalArray, removedArray) {
        const removedArrayDict = {};
        for(const user of removedArray) {
            removedArrayDict[user.value] = true;
        }
        for(const user of originalArray) {
            if (!removedArrayDict[user.value]) {
                return user;
            }
        }
    }

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
