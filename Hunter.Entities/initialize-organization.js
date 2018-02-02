db.Organization.update({ "ID": "A" }, {
    "ID": "A",
    "Name": "全部",
    "ParentID": null
}, { upsert: true })

db.Organization.update({ "ID": "A-1" }, {
    "ID": "A-1",
    "Name": "工程科",
    "ParentID": "A"
}, { upsert: true })

db.Organization.update({ "ID": "A-1-1" }, {
    "ID": "A-1-1",
    "Name": "工程一科",
    "ParentID": "A-1"
}, { upsert: true })

db.Organization.update({ "ID": "A-1-2" }, {
    "ID": "A-1-2",
    "Name": "工程二科",
    "ParentID": "A-1"
}, { upsert: true })

db.Organization.update({ "ID": "A-1-3" }, {
    "ID": "A-1-3",
    "Name": "工程三科",
    "ParentID": "A-1"
}, { upsert: true })

db.Organization.update({ "ID": "A-1-1-1" }, {
    "ID": "A-1-1-1",
    "Name": "科长",
    "ParentID": "A-1-1"
}, { upsert: true })

db.Organization.update({ "ID": "A-1-2-1" }, {
    "ID": "A-1-2-1",
    "Name": "科长",
    "ParentID": "A-1-2"
}, { upsert: true })

db.Organization.update({ "ID": "A-1-3-1" }, {
    "ID": "A-1-3-1",
    "Name": "科长",
    "ParentID": "A-1-2"
}, { upsert: true })

db.Organization.update({ "ID": "A-1-1-1-1" }, {
    "ID": "A-1-1-1-1",
    "Name": "科员",
    "ParentID": "A-1-1-1"
}, { upsert: true })

db.Organization.update({ "ID": "A-1-2-1-1" }, {
    "ID": "A-1-2-1-1",
    "Name": "科员",
    "ParentID": "A-1-2-1"
}, { upsert: true })

db.Organization.update({ "ID": "A-1-3-1-1" }, {
    "ID": "A-1-3-1-1",
    "Name": "科员",
    "ParentID": "A-1-2-1"
}, { upsert: true })