--[[
    GD50
    Super Mario Bros. Remake

    Author: Colton Ogden
    cogden@cs50.harvard.edu
]]

PlayerJumpState = Class{__includes = BaseState}

function PlayerJumpState:init(player, gravity)
    self.player = player
    self.gravity = gravity
    self.animation = Animation {
        frames = {3},
        interval = 1
    }
    self.player.currentAnimation = self.animation
end

function PlayerJumpState:enter(params)
    gSounds['jump']:play()
    self.player.dy = PLAYER_JUMP_VELOCITY
end

function PlayerJumpState:update(dt)
    self.player.currentAnimation:update(dt)
    self.player.dy = self.player.dy + self.gravity
    self.player.y = self.player.y + (self.player.dy * dt)

    -- go into the falling state when y velocity is positive
    if self.player.dy >= 0 then
        self.player:changeState('falling')
    end

    self.player.y = self.player.y + (self.player.dy * dt)

    -- look at two tiles above our head and check for collisions; 3 pixels of leeway for getting through gaps
    local tileLeft = self.player.map:pointToTile(self.player.x + 3, self.player.y)
    local tileRight = self.player.map:pointToTile(self.player.x + self.player.width - 3, self.player.y)

    -- if we get a collision up top, go into the falling state immediately
    if (tileLeft and tileRight) and (tileLeft:collidable() or tileRight:collidable()) then
        self.player.dy = 0
        self.player:changeState('falling')

    -- else test our sides for blocks
    elseif love.keyboard.isDown('left') then
        self.player.direction = 'left'
        self.player.x = self.player.x - PLAYER_WALK_SPEED * dt
        self.player:checkLeftCollisions(dt)
    elseif love.keyboard.isDown('right') then
        self.player.direction = 'right'
        self.player.x = self.player.x + PLAYER_WALK_SPEED * dt
        self.player:checkRightCollisions(dt)
    end

    -- check if we've collided with any collidable game objects
    for k, object in pairs(self.player.level.objects) do
        if object:collides(self.player) then
            if object.solid then
                object.onCollide(object)
                self.player.y = object.y + object.height
                self.player.dy = 0
                self.player:changeState('falling')
                -- check if lock block and remove if hit
                if object.lock and object.key == true then
                    local goalFrame = object.frame - 4
                    table.remove(self.player.level.objects, k)
                    -- once lock is gone add flagpole objects to object list
                    for x = 0, 2 do
                        table.insert(self.player.level.objects, 
                            GameObject{
                                texture = 'flags',
                                x = (self.player.mapWidth * 16) - 32,
                                y = VIRTUAL_HEIGHT - 96 + (16 * x),
                                width = 16,
                                height = 16,
                                frame = 2 + (x * 9) + goalFrame,
                                collidable = true,
                                hit = false,
                                solid = true,
                                consumable = false,
                                flag = true,
                                onCollide = function(obj)
                                    self.player.level:clear()
                                    -- restart playstate with new params for next level
                                    gStateMachine:change('play', {
                                        score = self.player.score,
                                        width = self.player.mapWidth + WIDTH_INCREMENT
                                    })
                                end   
                            })
                    end
                    -- check color of flag post to make same color flag
                    if goalFrame > 1 then
                        goalFrame = (9 * (goalFrame - 1)) + 7
                    else
                        goalFrame = goalFrame + 7
                    end
                    -- insert flag into object list
                    table.insert(self.player.level.objects, 
                            GameObject{
                                texture = 'flags',
                                x = (self.player.mapWidth * 16) - 20,
                                y = VIRTUAL_HEIGHT - 96,
                                width = 16,
                                height = 16,
                                frame = goalFrame,
                                collidable = true,
                                hit = false,
                                solid = true,
                                consumable = false,
                                flag = true,
                                onCollide = function(obj)
                                    self.player.level:clear()
                                     -- restart playstate with new params for next level
                                    gStateMachine:change('play', {
                                        score = self.player.score,
                                        width = self.player.mapWidth + WIDTH_INCREMENT
                                    })
                                end   
                            })
                end
                -- check for solid collision with flags
                if object.flag then
                    object.onCollide(object)
                end
            elseif object.consumable then
                object.onConsume(self.player)
                table.remove(self.player.level.objects, k)
            end
        end
    end

    -- check if we've collided with any entities and die if so
    for k, entity in pairs(self.player.level.entities) do
        if entity:collides(self.player) then
            gSounds['death']:play()
            gStateMachine:change('start')
        end
    end
end