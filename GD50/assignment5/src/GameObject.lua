--[[
    GD50
    Legend of Zelda

    Author: Colton Ogden
    cogden@cs50.harvard.edu
]]

GameObject = Class{}

function GameObject:init(def, x, y)
    -- string identifying this object type
    self.type = def.type

    self.texture = def.texture
    self.frame = def.frame or 1

    -- whether it acts as an obstacle or not
    self.solid = def.solid

    -- whether it is a consumable
    self.consumable = def.consumable

    self.defaultState = def.defaultState
    self.state = self.defaultState
    self.states = def.states

    
    -- dimensions
    self.x = x
    self.y = y
    self.width = def.width
    self.height = def.height

    -- default empty collision callback
    self.onCollide = function() end

    self.onConsume = def.onConsume


    -- trackers for pick up
    self.pickedUp = def.pickedUp

    -- for rendering infront of player
    self.foreground = false

    -- values for throwing and destruction of pot
    self.direction = 'down'
    self.throwSpeed = 80
    self.thrown = false
    self.distance = 0
    self.broken = false
end

function GameObject:update(dt)
    -- moving the pot once thrown in players direction
    if self.thrown then
        if self.x <= 24 or self.x >= VIRTUAL_WIDTH - 48 or self.y <= 24 or self.y >= VIRTUAL_HEIGHT - 48 then
            self.distance = POT_DISTANCE
            self.broken = true
        elseif self.distance <= POT_DISTANCE then
            if self.direction == 'right' then
                self.x = self.x + self.throwSpeed * dt
                self.distance = self.distance + self.throwSpeed * dt
            elseif self.direction == 'left' then
                self.x = self.x - self.throwSpeed * dt
                self.distance = self.distance + self.throwSpeed * dt
            elseif self.direction == 'down' then
                self.y = self.y + self.throwSpeed * dt
                self.distance = self.distance + self.throwSpeed * dt
            elseif self.direction == 'up' then
                self.y = self.y - self.throwSpeed * dt
                self.distance = self.distance + self.throwSpeed * dt
            end
        -- once past full distance pot breaks
        elseif self.distance >= POT_DISTANCE then
            self.broken = true
        end
    end
end

function GameObject:render(adjacentOffsetX, adjacentOffsetY)
    love.graphics.draw(gTextures[self.texture], gFrames[self.texture][self.states[self.state].frame or self.frame],
        self.x + adjacentOffsetX, self.y + adjacentOffsetY)
end