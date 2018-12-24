local _class = {}

ClassType = { 
    class = 1,
    instance = 2,
}

local function CallFuncDown(tbl,name,...)
    local func = nil
    func = function(tbl,clz,...)
        for _, s in pairs(clz.super) do
            func(tbl,s,...)
        end
        if clz[name] then
            clz[name](tbl,...)
        end
    end
    func(tbl,tbl._class_type,...)
end

local function CallFuncUp(tbl,name,...)
    local func = nil
    func = function(tbl,clz,...)
        if clz[name] then
            clz[name](tbl,...)
        end
        for _, s in pairs(clz.super) do
            func(tbl,s,...)
        end
    end
    func(tbl,tbl._class_type,...)
end

function Singleton(classname,...)
    for i = 1, select("#",...) do
        assert(select(i,...),"Singleton "..classname.." create while super has nil table!")
    end

    assert(type(classname)=="string" and #classname > 0)
    local class_type = {}
    class_type.__init = false
    class_type.__delete = false
    class_type.__cname = classname
    class_type.__ctype = ClassType.class

    class_type.super = {...}
    class_type.new = function(...)
        local instance = rawget(class_type,"__instance")
        if instance ~= nil then
            error(class_type.__cname.. "'s Instance is alrady exist!")
            return
        end

        local obj = {}
        obj._class_type = class_type
        obj.__ctype = ClassType.instance

        setmetatable(obj,{
            __index = _class[class_type],
        })

        CallFuncDown(obj,"__init",...)
        return obj
    end
    class_type.GetInstance = function()
        local instance = rawget(class_type,"__instance")
        if instance == nil then
            instance = class_type.new()
            rawset(class_type,"__instance",instance)
        end
        return instance
    end

    class_type.Destroy = function()
        local instance = rawget(class_type,"__instance")
        if instance then
            CallFuncUp(instance,"__delete")
            CallFuncUp(instance,"__destroy")
        end
        rawset(class_type,"__instance",nil)
    end

    local vtbl = {}
    assert(_class[class_type] == nil,"Aready defined class:",classname)
    _class[class_type] = vtbl
    setmetatable(class_type,{
        __newindex = function(t,k,v)
            vtbl[k] = v
        end
        ,
        __index = vtbl,
    }) 

    if class_type.super then 
        setmetatable(vtbl,{
            __index = function(t,k)
                if k == "Instance" then
                    return class_type.GetInstance()
                end
                for _, su in pairs(class_type.super) do
                    local ret = _class[su][k]
                    if ret then
                        return ret
                    end
                end
            end
        })
    end
    return class_type
end

function BaseClass(classname,...)
    for i=1,select("#",...) do
        assert(select(i,...),"Singleton "..classname.." create while super has nil table!")
    end

    assert(type(classname) == "string" and #classname > 0)
    local class_type = {}
    class_type.__init = false
    class_type.__delete = false
    class_type.__cname = classname
    class_type.__ctype = ClassType.class
    
    class_type.super = {...}
    class_type.new = function(...)
        local obj = {}
        obj._class_type = class_type
        obj.__ctype = ClassType.instance
        setmetatable(obj,{
            __index = _class[class_type],
        })

        obj.Clear = function(self)
            CallFuncUp(self,"__delete")
            CallFuncUp(self,"__destroy")
        end

        CallFuncDown(obj,"__init",...)
        return obj
    end
    local vtbl = {}
    assert(_class[class_type] == nil,"Aready defined class:",classname)
    _class[class_type] = vtbl

    setmetatable(class_type,{
        __newindex = function(t,k,v)
            vtbl[k]=v
        end
        ,
        __index = vtbl,
    })

    if class_type.super then
        setmetatable(vtbl,{
            __index = function(t,k)
                for _,su in pairs(class_type.super) do
                    local ret = _class[su][k]
                    if ret then
                        return ret
                    end
                end
            end
        })
    end
    return class_type
end

function getcname(tbl)
    if tbl.__ctype == ClassType.instance then
        return tbl._class_type.__cname
    else
        return tbl.__cname
    end
end